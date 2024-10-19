using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] BoardDisplayer boardDisplayer;
    [SerializeField] Editor_Controller editorController;
    //[SerializeField] PiecesInstantiator piecesInstantiator;
    public Board gameBoard;
    //public Tile[,] Board;
    [Header("Game")]
    //public int CurrentTeam = 0;
    [SerializeField] int startingTeamIndex;
    [SerializeField] bool startPlayingTrigger;
    Piece currentSelectedPiece;
    
    
    [Serializable]
    
    public class GameState
    {
        public bool isGameOver;
        public TeamClass Winner;
        public GameState(bool isover, TeamClass winner = null)
        {
            isGameOver = isover;
            Winner = winner;
        }
    }
    private void Update()
    {
        if (startPlayingTrigger) { StartPlaying(); startPlayingTrigger = false; }
    }
    public void StartPlaying()
    {
        //int width = piecesInstantiator.startingBoard.Width;
        //int height = piecesInstantiator.startingBoard.Height;

        //gameBoard = new Board(width, height, piecesInstantiator.startingBoard.AllTeams, startingTeamIndex, false);
        gameBoard = editorController.EditorToBoard(editorController.MainEditorBoard);

        gameBoard.OnMovedPieces += onMoved;

        Editor_Controller.CreatePieces(editorController.MainEditorBoard.PiecesToSpawn,gameBoard);

        editorController.destroyEditorTiles();
        editorController.DestroyInstantiatedPieces();
        boardDisplayer.DisplayBoard(gameBoard);

        foreach (TeamClass list in gameBoard.AllTeams)
        {
            SetPiecesSelectable(ref list.piecesList, false);
        }
        gameBoard.CurrentTeam = startingTeamIndex;

        onSelecting();
    }
    void SetPiecesSelectable(ref List<Piece> pieces, bool b)
    {
        foreach(Piece piece in pieces)
        {
            piece.isSelectable = b;
        }
    }
    public void onSelecting()
    {
        Debug.Log(gameBoard.AllTeams[gameBoard.CurrentTeam].TeamName + "'s turn to move");
        SetPiecesSelectable(ref gameBoard.AllTeams[gameBoard.CurrentTeam].piecesList, true);
        boardDisplayer.UpdatePieces(gameBoard, null);

        gameBoard.lastMovedPiece = null;
    }
    public void onPieceSelected()
    {

    }
    public void onMoved()
    {
        
        SetPiecesSelectable(ref gameBoard.AllTeams[gameBoard.CurrentTeam].piecesList, false);

        goToNextTeam();

        GameState currentResult = GetBoardState();
        if (currentResult.isGameOver)
        {
            Debug.Log("GAME OVER - WINNER: " + currentResult.Winner.TeamName);
            return;
        }
        if (gameBoard.AllTeams[gameBoard.CurrentTeam].isDefeated)
        {
            goToNextTeam();
        }
        
        onSelecting();
    }
    void goToNextTeam()
    {
        if (gameBoard.CurrentTeam == gameBoard.AllTeams.Count - 1)
        {
            gameBoard.CurrentTeam = 0;
        }
        else
        {
            gameBoard.CurrentTeam++;
        }
        if (gameBoard.AllTeams[gameBoard.CurrentTeam].isDefeated) { goToNextTeam(); }
    }
    GameState GetBoardState()
    {
        //IF A PLAYERS TURN BEGINS AND IT HAS NO AVAILABLE MOVEMENTS IT IS DEFEATED
        //AFTER THAT ITS TURN IS SKIPPED AND ITS PIECES ARE NOT CONSIDERED DANGEROUS ANYMORE

        //UNA GUARRADA ESTE SCRIPT UNA MICA

        //Look for No pieces
        for (int i = 0; i < gameBoard.AllTeams.Count; i++)
        {
            if (gameBoard.AllTeams[i].isDefeated) { continue; }

            if (gameBoard.AllTeams[i].piecesList.Count == 0)
            {
                Debug.Log(gameBoard.AllTeams[i].TeamName + " has no more pieces, so its defeated. RIP");
                gameBoard.AllTeams[i].OnDefeated();
                continue;
            }
        }

        if (!gameBoard.AllTeams[gameBoard.CurrentTeam].isDefeated)
        {
            //Look for checkMate
            if (gameBoard.isPlayerInCheck(gameBoard.CurrentTeam))
            {
                Debug.Log(gameBoard.AllTeams[gameBoard.CurrentTeam].TeamName + " is in check");
                if (gameBoard.isCurrentPlayerInCheckMate())
                {
                    Debug.Log(gameBoard.AllTeams[gameBoard.CurrentTeam].TeamName + " got CheckMated. RIP");
                    gameBoard.AllTeams[gameBoard.CurrentTeam].OnDefeated();
                }
            }
            //Look for DRAW
            else if (!gameBoard.canTeamMove(gameBoard.CurrentTeam))
            {
                Debug.Log(gameBoard.AllTeams[gameBoard.CurrentTeam].TeamName + " can't move. That should be a draw but in my rules its a defeat. RIP");
                gameBoard.AllTeams[gameBoard.CurrentTeam].OnDefeated();
            }
        }
        //Is there only one alive??
        List<int> teamsAlive = new List<int>();
        for (int i = 0; i < gameBoard.AllTeams.Count; i++)
        {
            if (!gameBoard.AllTeams[i].isDefeated) { teamsAlive.Add(i); }
        }
        if (teamsAlive.Count == 1) { boardDisplayer.UpdatePieces(gameBoard,null); return new GameState(true, gameBoard.AllTeams[teamsAlive[0]]);  }
        else return new GameState(false);
    }
    public void TileClicked(Tile tile)
    {
        Debug.Log("clicked Tile: " + tile.Coordinates + "tile is free? " + tile.isFree);
        if (currentSelectedPiece != null) //si tens una pessa seleccionada, deseleccionala
        {
            if(tile.isHighlighted)
            {
                gameBoard.AddMovement(new Board.Movement(currentSelectedPiece.Position, tile.Coordinates, gameBoard.CurrentTeam)); //currentSelectedPiece.MovePiece(tile);
            }
            currentSelectedPiece.OnPieceUnselected();

            boardDisplayer.UpdateHighlighted(gameBoard, null);
        }
        if (!tile.isFree && tile.currentPiece.isSelectable) //Seleccionada nova pessa
        {
            Debug.Log("Selected piece: " + tile.currentPiece.ToString());
            currentSelectedPiece = tile.currentPiece;
            tile.currentPiece.OnPieceSelected();
            tile.currentPiece.onPieceSelectedEvent?.Invoke();

            boardDisplayer.UpdateHighlighted(gameBoard, tile);
        }
        
    }
    
    
   
}
