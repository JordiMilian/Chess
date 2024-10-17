using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static TeamClass;

[Serializable]
public class Board 
{
    public Tile[,] AllTiles;
    public int Height,Width;
    public int CurrentTeam;
    public Action OnMovedPieces;
    public bool isVirtual;
    public Piece lastMovedPiece;

    public List<TeamClass> AllTeams = new List<TeamClass>();
    public Board(int height, int width, List<TeamClass> teams, int currentTeam, bool isvirtual)
    {
        Width = width; Height = height; CurrentTeam = currentTeam; isVirtual = isvirtual;
        //Create tiles
        AllTiles = new Tile[width,height];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                AllTiles[w, h] = new Tile( new Vector2Int(w, h));
            }
        }
        
        //Create Teams
        for (int t = 0; t < teams.Count; t++)
        {
            if (teams[t].piecesList == null) { teams[t].piecesList = new List<Piece>(); } ;

            TeamClass newTeam = new TeamClass(teams[t].TeamName,
                teams[t].PiecesColor,
                teams[t].isDefeated,
                teams[t].directionEnum
                );
            AllTeams.Add( newTeam);
        }
        //Fill with enemeis
        for (int t = 0; t < teams.Count; t++)
        {
            AllTeams[t].FillTeamWithEnemies(teams[t].piecesList, this);
        }
        UpdateKingsIndex();
        BoardDebugger.Log("Created new Board", this);
    }
    public class Movement
    { 
        public Vector2Int startPos;
        public Vector2Int endPos;
        public int team;
        public Movement(Vector2Int start, Vector2Int end, int thisteam)
        {
            startPos = start; endPos = end; team = thisteam;
        }
        public bool isMoveDoable(Board board)
        {
            if (startPos.x > board.Width || startPos.y > board.Height) { Debug.LogError("Starting position out of board: " + startPos); return false; }
            if (endPos.x > board.Width || endPos.y > board.Height) { Debug.LogError("End position out of board: " + endPos); return false; }
            if (board.AllTiles[startPos.x, startPos.y].isFree) { Debug.LogError("No piece in " + startPos); return false; }
            if (!board.AllTiles[endPos.x, endPos.y].isFree && board.AllTiles[endPos.x, endPos.y].currentPiece.Team == team) { Debug.LogError("Trying to eat teammate wtf"); return false; }
            return true;
        }
        public bool isMoveSaveFromCheck(Board board)
        {
            Board futureBoard = new Board(board.Height, board.Width,board.AllTeams,board.CurrentTeam,true);
            
            futureBoard.AddMovement(this);
            if (futureBoard.isPlayerInCheck(board.CurrentTeam)) { return false; }
            return true;
        }
    }
    public void AddMovement(Movement mov)
    {
        if (!mov.isMoveDoable(this)) { Debug.Log("Tried to make not doable movement");return; }

        //AllTiles[mov.startingPos.x, mov.startingPos.y].currentPiece.MovementUpdateInfo(AllTiles[mov.endPos.x, mov.endPos.y]);
        
        Tile newTile = AllTiles[mov.endPos.x, mov.endPos.y];
        Tile oldTile = AllTiles[mov.startPos.x, mov.startPos.y];
        Piece movedPiece = oldTile.currentPiece;

        if(!newTile.isFree)
        {
            Piece eatenPiece = AllTiles[mov.endPos.x, mov.endPos.y].currentPiece;
            //If the king got eaten RIP, however this should no be posible normally
            if(AllTeams[eatenPiece.Team].KingIndex > -1)
            {
                if (eatenPiece.currentTile.Coordinates == AllTeams[eatenPiece.Team].piecesList[AllTeams[eatenPiece.Team].KingIndex].Position)
                { AllTeams[eatenPiece.Team].OnDefeated(); }
                BoardDebugger.Log(AllTeams[eatenPiece.Team].TeamName + " got defeated by getting its King eaten WTF?" , this);
            }
            eatenPiece.onGettingEaten(); 
            
        }

        movedPiece.UpdateOwnTile(newTile);
        oldTile.UpdateTile(true, null);
        newTile.UpdateTile(false, movedPiece);

        BoardDebugger.Log("Moved " + AllTeams[movedPiece.Team].TeamName + "'s " + movedPiece.GetType().ToString() +
            " from " + oldTile.Coordinates + " to " + newTile.Coordinates,this);

        lastMovedPiece = movedPiece;
        movedPiece.CallMovedEvent();
        OnMovedPieces?.Invoke();
        
    }
    public bool canTeamMove(int Team)
    {
        for (int i = 0; i < AllTeams[Team].piecesList.Count; i++)
        {
            Piece thisPiece = AllTeams[Team].piecesList[i];
            if (thisPiece.GetAllPosibleMovements(thisPiece.currentTile.Coordinates).Length > 0)
            {
                return true;
            }
        }
        return false;
    }
    public bool isPlayerInCheck(int team)
    {
        if (AllTeams[team].KingIndex == -1) 
        { 
            //BoardDebugger.Log(AllTeams[team].TeamName + " has no king so no need to check",this); 
            return false; }
        for (int t = 0; t < AllTeams.Count; t++)
        {
            if(t == team) { continue; }
            
            for (int p = 0; p < AllTeams[t].piecesList.Count; p++)
            {
                Tile[] dangerousTiles = AllTeams[t].piecesList[p].GetDangerousTiles();
                foreach (Tile  til in dangerousTiles)
                {
                    if(til.Coordinates == AllTeams[team].piecesList[AllTeams[team].KingIndex].Position) 
                    {
                        //BoardDebugger.Log(AllTeams[team].TeamName + " is in check because King is in: " + AllTeams[team].piecesList[AllTeams[team].KingIndex].Position, this);
                        return true; 
                    }
                }
            }
        }
        return false;
    }
    public bool isCurrentPlayerInCheckMate()
    {
        float startingTime = Time.time;
        
        for (int p = 0; p < AllTeams[CurrentTeam].piecesList.Count; p++)
        {
            if (AllTeams[CurrentTeam].piecesList[p].GetAllPosibleMovements(AllTeams[CurrentTeam].piecesList[p].Position).Length > 0)
            {
                return false;
            }
        }
        return true;
       
    }
    public void UpdateKingsIndex()
    {
        for (int i = 0; i < AllTeams.Count; i++)
        {
            AllTeams[i].KingIndex = -1;
            for (int p = 0; p < AllTeams[i].piecesList.Count; p++)
            {
                if (AllTeams[i].piecesList[p].pieceEnum == Piece.PiecesEnum.Rei)
                {
                    AllTeams[i].KingIndex = p;
                    BoardDebugger.Log(AllTeams[i].TeamName +  " King is in index: " + p, this);
                    break;
                }
            }
        }
    }
    static Vector2Int enumToVector(directions dir)
    {
        switch (dir)
        {
            case directions.up: return Vector2Int.up;
            case directions.down: return Vector2Int.down;
            case directions.left: return Vector2Int.left;
            case directions.right: return Vector2Int.right;
        }
        return Vector2Int.zero;
    }
}
