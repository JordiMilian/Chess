using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Board 
{
    public Tile[,] AllTiles;
    public int Height,Width;
    public int CurrentTeam;
    [Serializable]
    public class TeamClass
    {
        public string TeamName;
        public List<Piece> piecesList = new List<Piece>();
        public Color PiecesColor;
        public int Direction = 1;
        public bool isDefeated;
        public Piece King;
        public TeamClass(string name, Color color, int direction)
        {
            TeamName = name;
            PiecesColor = color;
            Direction = direction;
        }
    }
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
        Width = width; Height = height; CurrentTeam = currentTeam; AllTeams = teams;
        //Set Pieces
        for (int t = 0; t < teams.Count; t++)
        {
            for (int p = 0; p < teams[t].piecesList.Count; p++)
            {
                Piece thisPiece = teams[t].piecesList[p];
                thisPiece.currentTile = AllTiles[thisPiece.Position.x, thisPiece.Position.y];
                AllTiles[thisPiece.Position.x, thisPiece.Position.y].currentPiece = thisPiece;
            }
        }
    }
    public class Movement
    { 
        public Vector2Int startingPos;
        public Vector2Int endPos;
        public int team;
        public Movement(Vector2Int start, Vector2Int end, int thisteam)
        {
            startingPos = start; endPos = end; team = thisteam;
        }

    }
    public void AddMovement(Movement mov)
    {
        if (isMovementLegal(mov)) { Debug.Log("Tried to make illegal movement");return; }
        AllTiles[mov.startingPos.x, mov.startingPos.y].currentPiece.MovePiece(AllTiles[mov.endPos.x, mov.endPos.y]);
    }
    public bool isMovementLegal(Movement mov)
    {
        if(mov.startingPos.x > Width || mov.startingPos.y > Height) { Debug.LogError("Starting position out of board: " + mov.startingPos); return false; }
        if(mov.endPos.x > Width || mov.endPos.y > Height) { Debug.LogError("End position out of board: " + mov.endPos); return false; }
        if (AllTiles[mov.startingPos.x, mov.startingPos.y].isFree) { Debug.LogError("No piece in " + mov.startingPos); return false; }
        if (!AllTiles[mov.endPos.x, mov.endPos.y].isFree && AllTiles[mov.endPos.x, mov.endPos.y].currentPiece.Team == mov.team) { Debug.LogError("Trying to eat teammate wtf"); return false; }
        return true;
    }
    public bool canTeamMove(int Team)
    {
        for (int i = 0; i < AllTeams[Team].piecesList.Count; i++)
        {
            Piece thisPiece = AllTeams[Team].piecesList[i];
            if (thisPiece.GetMovableTiles(thisPiece.currentTile.Coordinates).Length > 0)
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
                Tile[] thisPiecesTiles = AllTeams[t].piecesList[p].GetMovableTiles(AllTeams[t].piecesList[p].currentTile.Coordinates); //Should be GetDangerousTiles
                foreach (Tile tile in thisPiecesTiles)
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
        if (AllTeams[CurrentTeam].King == null) { Debug.Log("Current Team has no king so no check"); return false; }
        for (int t = 0; t < AllTeams.Count; t++)
        {
            if(t == CurrentTeam) { continue; }
            
            for (int p = 0; p < AllTeams[t].piecesList.Count; p++)
            {
                Tile[] dangerousTiles = AllTeams[t].piecesList[p].GetMovableTiles(AllTeams[t].piecesList[p].Position);
                foreach (Tile tile in dangerousTiles)
                {
                    if(tile.Coordinates == AllTeams[CurrentTeam].King.Position) { return true; }
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
                Tile[] dangerousTiles = AllTeams[t].piecesList[p].GetMovableTiles(AllTeams[t].piecesList[p].Position);

                foreach (Tile tile in dangerousTiles)//iterar cada posicio valida de la pessa
                {
                    //Crear un tauler nou futur i simular nova posicio
                    Board futureBoard = new Board(Height, Width, AllTeams, CurrentTeam);
                    futureBoard.AddMovement(new Movement(AllTeams[t].piecesList[p].Position, tile.Coordinates,t));
                    if (futureBoard.isBoardInCheck()) { continue; }
                    else { Debug.Log("Calculations time: " + (Time.time - startingTime)); return false;  }
                }
            }
        }
        Debug.Log("Calculations time: " + (Time.time - startingTime));
        return true;
    }
}
