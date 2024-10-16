using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] BoardDisplayer boardDisplayer;
    [SerializeField] PiecesInstantiator piecesInstantiator;
    public Board gameBoard;
    //public Tile[,] Board;
    [Header("Game")]
    //public int CurrentTeam = 0;
    [SerializeField] int startingTeamIndex;
    [SerializeField] bool updateVisualsTrigger;
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
        if (updateVisualsTrigger) { boardDisplayer.UpdatePieces(gameBoard); updateVisualsTrigger = false; }
    }
    private void Awake()
    {
        //Tot aixo no estic segur si es necesari de fer pero bueno. Basicament estic crean un nou Board amb la info del Board serialitzat
        int width = piecesInstantiator.startingBoard.Width;
        int height = piecesInstantiator.startingBoard.Height;
        /*
        List<TeamClass> allTeams = new List<TeamClass>();
        for (int i = 0; i < gameBoard.AllTeams.Count; i++)
        {
            if (gameBoard.AllTeams[i].piecesList == null) { gameBoard.AllTeams[i].piecesList = new List<Piece>(); }

            allTeams.Add( new TeamClass(
                gameBoard.AllTeams[i].TeamName,
                gameBoard.AllTeams[i].PiecesColor,
                gameBoard.AllTeams[i].Direction,
                gameBoard.AllTeams[i].piecesList,
                gameBoard
                ));
        }
        */
        gameBoard = new Board(width,height,piecesInstantiator.startingBoard.AllTeams,startingTeamIndex);

        gameBoard.OnMovedPieces += onMoved;
        
    }
    private void Start()
    {
        piecesInstantiator.CreatePieces();

        boardDisplayer.DisplayBoard(gameBoard);

        foreach(TeamClass list in gameBoard.AllTeams)
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
    }
    public void onSelected()
    {
    }
    public void onMoved()
    {
        boardDisplayer.UpdatePieces(gameBoard);
        SetPiecesSelectable(ref gameBoard.AllTeams[gameBoard.CurrentTeam].piecesList, false);

        goToNextTeam();

        GameState currentResult = checkBoardState();
        if (currentResult.isGameOver)
        {
            Debug.Log("GAME OVER - WINNER: " + currentResult.Winner.TeamName);
            return;
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
    GameState checkBoardState()
    {
        List<int> teamsAlive = new List<int>();
        //If ran out of pieces
        for (int i = 0; i < gameBoard.AllTeams.Count; i++)
        {
            if (gameBoard.AllTeams[i].piecesList.Count == 0) { continue; }
            teamsAlive.Add(i);
        }
        //If current team cannot move (current team has to move now)
        //if (!gameBoard.canTeamMove(CurrentTeam)) { teamsAlive.Remove(CurrentTeam); }

        if(teamsAlive.Count == 1) { return new GameState(true, gameBoard.AllTeams[teamsAlive[0]]); }
        else return new GameState(false);
    }
    public void TileClicked(Tile tile)
    {
        string currentPieceTxt = " with no current piece";
        if (!tile.isFree) { currentPieceTxt = tile.currentPiece.GetType().ToString(); }
        Debug.Log("clicked tile: " + tile.Coordinates + currentPieceTxt);
        if (currentSelectedPiece != null)
        {
            if(tile.isHighlighted)
            {
                gameBoard.AddMovement(new Board.Movement(currentSelectedPiece.Position, tile.Coordinates, gameBoard.CurrentTeam)); //currentSelectedPiece.MovePiece(tile);
            }
            currentSelectedPiece.OnPieceUnselected();
        }
        if (!tile.isFree && tile.currentPiece.isSelectable)
        {
            Debug.Log("Selected piece: " + tile.currentPiece.ToString());
            currentSelectedPiece = tile.currentPiece;
            tile.currentPiece.OnPieceSelected();
            tile.currentPiece.onPieceSelectedEvent?.Invoke();
        }

        boardDisplayer.UpdateHighlighted(gameBoard);
    }
    
    
   
}
