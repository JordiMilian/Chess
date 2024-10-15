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
    public int CurrentTeam = 0;
    [SerializeField] int startingTeamIndex;
    
    Piece currentSelectedPiece;

    
    [Serializable]
    
    public class GameState
    {
        public bool isGameOver;
        public Board.TeamClass Winner;
        public GameState(bool isover, Board.TeamClass winner = null)
        {
            isGameOver = isover;
            Winner = winner;
        }
    }

    
    private void Awake()
    {
        //Tot aixo no estic segur si es necesari de fer pero bueno. Basicament estic crean un nou Board amb la info del Board serialitzat
        int width = gameBoard.Width;
        int height = gameBoard.Height;
        List<Board.TeamClass> allTeams = new List<Board.TeamClass>();
        for (int i = 0; i < gameBoard.AllTeams.Count; i++)
        {
            allTeams.Add( new Board.TeamClass(
                gameBoard.AllTeams[i].TeamName,
                gameBoard.AllTeams[i].PiecesColor,
                gameBoard.AllTeams[i].Direction
                ));
        }
        gameBoard = new Board(width,height,allTeams,startingTeamIndex);
        
    }
    private void Start()
    {
        piecesInstantiator.CreatePieces();

        boardDisplayer.DisplayBoard(gameBoard);

        foreach(Board.TeamClass list in gameBoard.AllTeams)
        {
            SetPiecesSelectable(ref list.piecesList, false);
        }
        CurrentTeam = startingTeamIndex;
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
        Debug.Log(gameBoard.AllTeams[CurrentTeam].TeamName + "'s turn to move");
        SetPiecesSelectable(ref gameBoard.AllTeams[CurrentTeam].piecesList, true);
    }
    public void onSelected()
    {
    }
    public void onMoved()
    {
        SetPiecesSelectable(ref gameBoard.AllTeams[CurrentTeam].piecesList, false);

        goToNextTeam();

        GameState currentResult = checkBoardState();
        if (currentResult.isGameOver)
        {
            Debug.Log("GAME OVER - WINNER: " + currentResult.Winner);
            return;
        }

        onSelecting();
    }
    void goToNextTeam()
    {
        if (CurrentTeam == gameBoard.AllTeams.Count - 1)
        {
            CurrentTeam = 0;
        }
        else
        {
            CurrentTeam++;
        }
        if (gameBoard.AllTeams[CurrentTeam].isDefeated) { goToNextTeam(); }

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
        if (!gameBoard.canTeamMove(CurrentTeam)) { teamsAlive.Remove(CurrentTeam); }

        if(teamsAlive.Count == 1) { return new GameState(true, gameBoard.AllTeams[teamsAlive[0]]); }
        else return new GameState(false);
    }
    void TileClicked(Tile tile)
    {
        Debug.Log("clicked tile: " + tile.Coordinates);
        if (currentSelectedPiece != null)
        {
            currentSelectedPiece.OnPieceUnselected();
        }
        if (!tile.isFree && tile.currentPiece.isSelectable)
        {
            Debug.Log("Selected piece: " + tile.currentPiece.ToString());
            currentSelectedPiece = tile.currentPiece;
            tile.currentPiece.OnPieceSelected();
            tile.currentPiece.onPieceSelectedEvent?.Invoke();
        }
    }
    
    
   
}
