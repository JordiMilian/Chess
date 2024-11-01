using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public Board GameBoard;
    [SerializeField] BoardDisplayer boardDisplayer;
    [SerializeField] Editor_Controller editorController;
    [SerializeField] Play_TextController textController;
    [SerializeField] TextCutscenes textCutscenes;
    [SerializeField] AudioClip startNextTurnAudio;
    public Coroutine currentCoroutine;

    Piece currentSelectedPiece;
    bool isBoardWithOnePlayer;
    bool isGameOver;
    
    [Serializable]
    public class GameState
    {
        public bool isGameOver;
        public TeamClass Winner;
        public int WinnerIndex;
        public List<int> DefeatedTeams;
        public List<int> TeamsInCheck;
        public List <string> reasonsOfDefeat;
        public GameState(bool isover, int winnerIndex = -1,TeamClass winner = null)
        {
            isGameOver = isover;
            Winner = winner;
            WinnerIndex = winnerIndex;
            DefeatedTeams = new List<int>();
            TeamsInCheck = new List<int>();
            reasonsOfDefeat = new List<string>();
        }
    }
    private void Start()
    {
        ReturnToEditing();
    }
    public void StartPlaying()
    {
        currentCoroutine = StartCoroutine(StartPlayingCoroutine());
    }
     IEnumerator StartPlayingCoroutine()
    {
        GameBoard = editorController.EditorToBoard(editorController.MainEditorBoard);
        GameBoard.OnMovedPieces += onMoved;

        Editor_Controller.CreatePieces(editorController.MainEditorBoard.PiecesToSpawn,GameBoard);

        editorController.StopEditing();
        boardDisplayer.DestroyCurrentBoard();
        boardDisplayer.DisplayBoard(GameBoard);
        textController.OnSettingUp();
        
        yield return StartCoroutine(boardDisplayer.appearTilesCutscene());
        yield return StartCoroutine(boardDisplayer.AppearPiecesCutscene());
        yield return new WaitForSeconds(1);

        foreach (TeamClass list in GameBoard.AllTeams)
        {
            SetPiecesSelectable(ref list.piecesList, false);
        }

        isBoardWithOnePlayer = false;
        isGameOver = false;
        int playersinBoard = CheckPlayingPlayers();
        if(playersinBoard == 1)
        {
            isBoardWithOnePlayer = true;
            Debug.Log("Solo player is: " + GameBoard.CurrentTeam);
            if (!GameBoard.canTeamMove(GameBoard.CurrentTeam))
            {
                GameBoard.AllTeams[GameBoard.CurrentTeam].OnDefeated();
                boardDisplayer.UpdatePieces(GameBoard,null);
                yield return StartCoroutine(textCutscenes.LostWithOnePlayer());
                
                yield break;
            }
            else
            {
                //yield return StartCoroutine(textCutscenes.OnlyOnePlayer());
            }
            
        }
        if(playersinBoard == 0)
        {
            textController.OnBoardWithNoTeams();
            yield return StartCoroutine(textCutscenes.EmptyBoardCoroutine());
            yield break;
        }
        

        GameBoard.CurrentTeam--;
        yield return StartCoroutine(endTurnCoroutine());
        
    }
    
    public void ReturnToEditing()
    {
        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
       
        boardDisplayer.HidePLayingStuff();
        editorController.LoadMainBoard();
    }
    void SetPiecesSelectable(ref List<Piece> pieces, bool b)
    {
        foreach(Piece piece in pieces)
        {
            piece.isSelectable = b;
        }
    }
    public IEnumerator StartNewTurn()
    {
       
        if (!isBoardWithOnePlayer) { textController.OnPlayersTurn(GameBoard.CurrentTeam); }

        if (GameBoard.AllTeams[GameBoard.CurrentTeam].isComputer)
        {
            SFX_PlayerSingleton.Instance.playSFX(startNextTurnAudio, 0.05f);

            yield return new WaitForSeconds(0.2f);
            
            Board.Movement chosenMove = GetComputerMovement();
            GameBoard.AddMovement(chosenMove);
            Piece MovedPiece = GameBoard.AllTiles[chosenMove.endPos.x, chosenMove.endPos.y].currentPiece;
            boardDisplayer.UpdatePieces(GameBoard,MovedPiece);
            yield break;
        }


        yield return null;


        SetPiecesSelectable(ref GameBoard.AllTeams[GameBoard.CurrentTeam].piecesList, true);
        SFX_PlayerSingleton.Instance.playSFX(startNextTurnAudio,0.05f);
        boardDisplayer.UpdatePieces(GameBoard, null);

        GameBoard.lastMovedPiece = null;
    }
    Board.Movement GetComputerMovement()
    {
        float startinTime = Time.time;
        Board.Movement[] legalMoves = GameBoard.GetAllLegalMovementsOfTeam(GameBoard.CurrentTeam);
        List<int> legalMovesIndexes = new List<int>();
        List<int> eatableMoves = new List<int>();
        List<int> checkableMoves = new List<int>();
        List<int> mateableMoves = new List<int>();
        List<int> unsafeMoves = new List<int>();

        for (int m = 0; m < legalMoves.Length; m++)
        {
            legalMovesIndexes.Add(m);
            Tile endTile = GameBoard.AllTiles[legalMoves[m].endPos.x, legalMoves[m].endPos.y];
            if(!endTile.isFree && endTile.currentPiece.Team != GameBoard.CurrentTeam)
            {
                eatableMoves.Add(m);
            }

            Board futureBoard = new Board(GameBoard.Height, GameBoard.Width, GameBoard.AllTeams, GameBoard.CurrentTeam, true);
            futureBoard.AddMovement(legalMoves[m]);
            List<Board.Movement> futureEnemyMoves = new List<Board.Movement>();
            for (int t = 0; t < futureBoard.AllTeams.Count; t++)
            {
                if(t == futureBoard.CurrentTeam) { continue; }
                if (futureBoard.AllTeams[t].isDefeated) { continue; }

                if (futureBoard.isPlayerInCheck(t))
                {
                    checkableMoves.Add(m);
                    if(futureBoard.isTeamInCheckMate(t))
                    {
                        mateableMoves.Add(m);
                    }
                }
                futureEnemyMoves.AddRange(futureBoard.GetAllLegalMovementsOfTeam(t));
            }
            for (int fem = 0; fem < futureEnemyMoves.Count; fem++)
            {
                if (unsafeMoves.Contains(m)) { break; }
                if(futureEnemyMoves[fem].endPos == legalMoves[m].endPos)
                {
                    unsafeMoves.Add(m);
                }
            }
        }
        int[] SaveChecks = getSafeMovesFromList(ref checkableMoves);
        int[] SaveEateables = getSafeMovesFromList(ref eatableMoves);
        int[] SaveLegals = getSafeMovesFromList(ref legalMovesIndexes);
        int chosenIndex = 0;
        if (mateableMoves.Count > 0) { chosenIndex = mateableMoves[UnityEngine.Random.Range(0, mateableMoves.Count - 1)]; }
        else if(SaveEateables.Length > 0 && SaveChecks.Length > 0)
        {
            int randomInt = UnityEngine.Random.Range(0, 9);
            if(randomInt % 2 == 0) { chosenIndex = SaveChecks[UnityEngine.Random.Range(0, SaveChecks.Length - 1)]; }
            else { chosenIndex = SaveEateables[UnityEngine.Random.Range(0, SaveEateables.Length - 1)]; }
        }
        else if(SaveChecks.Length > 0) 
        { 
            chosenIndex = SaveChecks[UnityEngine.Random.Range(0,SaveChecks.Length -1)];
        }
        else if(SaveEateables.Length > 0) 
        {
            chosenIndex = SaveEateables[UnityEngine.Random.Range(0, SaveEateables.Length - 1)];
        }
        else 
        {
            if (checkableMoves.Count > 0) { chosenIndex = checkableMoves[UnityEngine.Random.Range(0, checkableMoves.Count - 1)]; }
            else if (eatableMoves.Count > 0) { chosenIndex = eatableMoves[UnityEngine.Random.Range(0, eatableMoves.Count - 1)]; }
            else if (SaveLegals.Length > 0)
            {
                chosenIndex = SaveLegals[UnityEngine.Random.Range(0, SaveLegals.Length - 1)];
            }
            else
            {
                chosenIndex = legalMovesIndexes[UnityEngine.Random.Range(0, legalMovesIndexes.Count - 1)];
            }
            
        }
        float calculationTime = Time.time - startinTime;
        Debug.Log("mateables moves: " + mateableMoves.Count +
            " - checkeable moves: " + SaveChecks.Length + "/" + checkableMoves.Count +
            " - eatable moves: " + SaveEateables.Length + "/" + eatableMoves.Count +
            " - legalMoves" + SaveLegals.Length + "/" + legalMovesIndexes.Count +
            " - calculation Time: " + calculationTime
            ) ;


        return legalMoves[chosenIndex];

        int[] getSafeMovesFromList(ref List<int> intList)
        {
            List<int> safeMoves = new List<int>();
            for (int i = 0; i < intList.Count; i++)
            {
                if (unsafeMoves.Contains(intList[i])) { continue; }
                safeMoves.Add(intList[i]);
            }
            return safeMoves.ToArray();
        }
    }
    public void onMoved()
    {
        currentCoroutine = StartCoroutine(endTurnCoroutine());
    }
    IEnumerator endTurnCoroutine()
    {
        if (GameBoard.CurrentTeam > -1) { SetPiecesSelectable(ref GameBoard.AllTeams[GameBoard.CurrentTeam].piecesList, false); }
        boardDisplayer.UpdatePieces(GameBoard, null);
        goToNextTeam();

        yield return StartCoroutine(ReadStateCoroutine());

        if (GameBoard.AllTeams[GameBoard.CurrentTeam].isDefeated && !isGameOver)
        {
            Debug.Log("current player died, checking agains");
            yield return StartCoroutine(endTurnCoroutine());
            yield break;
        }
        if(isGameOver && !isBoardWithOnePlayer)
        {
            Debug.Log("Game over, should not keep playing");
            yield break;
            
        }

        currentCoroutine = StartCoroutine( StartNewTurn());
    }
    IEnumerator ReadStateCoroutine()
    {
        GameState currentState = GetGameState();
        isGameOver = currentState.isGameOver;

        for (int i = 0; i < currentState.DefeatedTeams.Count; i++)
        {
            GameBoard.AllTeams[currentState.DefeatedTeams[i]].OnDefeated();
        }
        boardDisplayer.UpdatePieces(GameBoard, null);

        for (int d = 0; d < currentState.DefeatedTeams.Count; d++)
        {
            if(isBoardWithOnePlayer) //Loosing with only one player
            {
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(textCutscenes.LostWithOnePlayer());

                yield break;
            }

            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(textCutscenes.PlayerDefeatedCoroutine(
                currentState.DefeatedTeams[d],
                currentState.reasonsOfDefeat[d]
                ));
            yield return new WaitForSeconds(.5f);
        }
        if (!isBoardWithOnePlayer)
        {
            if (currentState.isGameOver)
            {
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(textCutscenes.GameOverCutscene(currentState.WinnerIndex));

                yield break;
            }
        }
        
    }

    GameState GetGameState()
    {
        GameState endTurnState = new GameState(false);

        for (int i = 0; i < GameBoard.AllTeams.Count; i++)
        {
            if (GameBoard.AllTeams[i].isDefeated) { continue; }

            textController.OnPlayerFine(i);

            //Look for No pieces
            if (GameBoard.AllTeams[i].piecesList.Count == 0)
            {
                endTurnState.DefeatedTeams.Add(i);
                endTurnState.reasonsOfDefeat.Add("No available pieces");
                GameBoard.AllTeams[i].isDefeated = true;
                continue;
            }
            //check all players check state
            if (GameBoard.isPlayerInCheck(i))
            {
                endTurnState.TeamsInCheck.Add(i);
            }
        }

        if (!GameBoard.AllTeams[GameBoard.CurrentTeam].isDefeated)
        {
            //Look for checkMate
            if (GameBoard.isPlayerInCheck(GameBoard.CurrentTeam))
            {
                if (GameBoard.isTeamInCheckMate(GameBoard.CurrentTeam))
                {
                    endTurnState.DefeatedTeams.Add(GameBoard.CurrentTeam);
                    endTurnState.reasonsOfDefeat.Add("Checkmate");
                    GameBoard.AllTeams[GameBoard.CurrentTeam].isDefeated = true;
                }
            }
            //Look for DRAW
            else if (!GameBoard.canTeamMove(GameBoard.CurrentTeam))
            {
                endTurnState.DefeatedTeams.Add(GameBoard.CurrentTeam);
                endTurnState.reasonsOfDefeat.Add("No available moves");
                GameBoard.AllTeams[GameBoard.CurrentTeam].isDefeated = true;
            }
        }

        //Only one player alive?
        List<int> teamsAlive = new List<int>();
        for (int i = 0; i < GameBoard.AllTeams.Count; i++)
        {
            if (!GameBoard.AllTeams[i].isDefeated) { teamsAlive.Add(i); }
        }
        if (teamsAlive.Count == 1)
        {
            endTurnState.isGameOver = true;
            endTurnState.WinnerIndex = teamsAlive[0];
            endTurnState.Winner = GameBoard.AllTeams[teamsAlive[0]];
        }

        return endTurnState;
    }
    void goToNextTeam()
    {  
        if (GameBoard.CurrentTeam == GameBoard.AllTeams.Count - 1)
        {
            GameBoard.CurrentTeam = 0;
        }
        else
        {
            GameBoard.CurrentTeam++;
        }

        if (GameBoard.AllTeams[GameBoard.CurrentTeam].isDefeated) 
        {
            int teamsAlive = 0;
            foreach (TeamClass team in GameBoard.AllTeams)
            {
                if (!team.isDefeated)
                {
                    teamsAlive++;
                }
            }
            if(teamsAlive > 0) { goToNextTeam(); }
            else { Debug.Log("No teams alive"); }
        }
        
        
    }

    public void TileClicked(Tile tile)
    {
        if (currentSelectedPiece != null) 
        {
            Board.Movement[] posibleMoves = currentSelectedPiece.GetAllLegalMoves();
            for (int m = 0; m < posibleMoves.Length; m++)
            {
                if (posibleMoves[m].endPos == tile.Coordinates)
                {
                    GameBoard.AddMovement(posibleMoves[m]);
                }
            }
            currentSelectedPiece.OnPieceUnselected();
            currentSelectedPiece = null;

            boardDisplayer.UpdateHighlighted(GameBoard, null);
        }
        if (!tile.isFree && tile.currentPiece.isSelectable) //Seleccionada nova pessa
        {
            Debug.Log("Selected piece: " + tile.currentPiece.ToString());
            currentSelectedPiece = tile.currentPiece;
            tile.currentPiece.OnPieceSelected();
            tile.currentPiece.onPieceSelectedEvent?.Invoke();

            boardDisplayer.UpdateHighlighted(GameBoard, tile);
        }
    }
    int CheckPlayingPlayers()
    {
        int boardTeams = 0;
        for (int i = 0; i < GameBoard.AllTeams.Count; i++)
        {
            if (GameBoard.AllTeams[i].piecesList.Count == 0)
            {
                textController.OnPlayerNotPlaying(i);
                GameBoard.AllTeams[i].OnDefeated();
                continue;
            }
            boardTeams++;
        }
        GameBoard.CurrentTeam--;
        goToNextTeam();
        return boardTeams;
    }
}
