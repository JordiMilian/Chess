using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Board 
{
    public Tile[,] AllTiles;
    public int Height,Width;
    public int CurrentTeam;
    public Action OnMovedPieces;

    public List<TeamClass> AllTeams = new List<TeamClass>();
    public Board(int height, int width, List<TeamClass> teams, int currentTeam)
    {
        //Create tiles
        AllTiles = new Tile[width,height];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                AllTiles[w, h] = new Tile( new Vector2Int(w, h));
            }
        }
        Width = width; Height = height; CurrentTeam = currentTeam;
        //Set Pieces
        //AllTeams = new List<TeamClass>(teams.Count);
        for (int t = 0; t < teams.Count; t++)
        {
            if (teams[t].piecesList == null) { teams[t].piecesList = new List<Piece>(); } ;
            TeamClass newTeam = new TeamClass(teams[t].TeamName,
                teams[t].PiecesColor,
                teams[t].Direction,
                teams[t].piecesList
                );
            AllTeams.Add( newTeam);
        }
        for (int t = 0; t < teams.Count; t++)
        {
            AllTeams[t].FillTeamWithEnemies(teams[t].piecesList, this);
        }
        Debug.Log("Created new Board");
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
            Board futureBoard = new Board(board.Height, board.Width,board.AllTeams,board.CurrentTeam);
            
            futureBoard.AddMovement(this);
            if (futureBoard.isBoardInCheck()) { return false; }
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
            AllTeams[eatenPiece.Team].piecesList.Remove(eatenPiece); 
        }

        movedPiece.UpdateOwnTile(newTile);
        oldTile.UpdateTile(true, null);
        newTile.UpdateTile(false, movedPiece);
        Debug.Log("Moved " + AllTeams[movedPiece.Team].TeamName + "'s " + movedPiece.GetType().ToString() +
            " from " + oldTile.Coordinates + " to " + newTile.Coordinates);
        Debug.Log("Moved piece is in: " + movedPiece.Position + " King is in: " + AllTeams[CurrentTeam].King.Position);

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
    public Tile[] GetAllDangerousTiles(int team)
    {
        List<Tile> dangerousTiles = new List<Tile>();
        for (int t = 0; t < AllTeams.Count; t++)
        {
            if (t == team) { continue; }
            for (int p = 0; p < AllTeams[t].piecesList.Count; p++)
            {
                Tile[] thisPiecesMoves = AllTeams[t].piecesList[p].GetDangerousTiles(); //Should be GetDangerousTiles
                foreach (Tile tile in thisPiecesMoves)
                {
                    
                    if (dangerousTiles.Contains(tile)) { continue; }
                    dangerousTiles.Add(tile);
                }
            }
        }
        return dangerousTiles.ToArray();
    }
    public bool isBoardInCheck()
    {
        if (AllTeams[CurrentTeam].King == null) { Debug.Log(AllTeams[CurrentTeam].TeamName + " has no king so no check"); return false; }
        for (int t = 0; t < AllTeams.Count; t++)
        {
            if(t == CurrentTeam) { continue; }
            
            for (int p = 0; p < AllTeams[t].piecesList.Count; p++)
            {
                Tile[] dangerousTiles = AllTeams[t].piecesList[p].GetDangerousTiles();
                foreach (Tile  til in dangerousTiles)
                {
                    if(til.Coordinates == AllTeams[CurrentTeam].King.Position) 
                    {
                        Debug.Log("Board is in check because King is in: "+ AllTeams[CurrentTeam].King.Position);
                        return true; 
                        
                    }
                }
            }
        }
        return false;
    }
    public bool isBoardCheckMate()
    {
        float startingTime = Time.time;
        for (int t = 0; t < AllTeams.Count; t++) //iterar els equips enemics
        {
            if (t == CurrentTeam) { continue; }

            for (int p = 0; p < AllTeams[t].piecesList.Count; p++) //iterar cada pessa enemiga
            {
                Movement[] dangerousMoves = AllTeams[t].piecesList[p].GetAllPosibleMovements(AllTeams[t].piecesList[p].Position);

                foreach (Movement mov in dangerousMoves)//iterar cada posicio valida de la pessa
                {
                    //Crear un tauler nou futur i simular nova posicio
                    Board futureBoard = new Board(Height, Width, AllTeams, CurrentTeam);
                    futureBoard.AddMovement(mov);
                    if (futureBoard.isBoardInCheck()) { continue; }
                    else { Debug.Log("Calculations time: " + (Time.time - startingTime)); return false;  }
                }
            }
        }
        Debug.Log("Calculations time: " + (Time.time - startingTime));
        return true;
    }
}
