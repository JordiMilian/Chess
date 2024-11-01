using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Editor_Controller;

[Serializable]
public class EditorBoard
{
    public int maxTileWidth = 16, maxTileHeight = 16;
    public Vector2Int maxActiveTiles;
    public EditorTile[,] allTiles;
    public List<PieceCreator> PiecesToSpawn;
    public int startingTeam;
    [HideInInspector] public TeamClass.directions[] teamsDirs = new TeamClass.directions[4];
     public bool[] areComputers;
    [Serializable]
    public class EditorTile
    {
        public Vector2Int Position;
        public bool isOcupied;
        public bool isActive;
        public PieceCreator creatorOcupying;

        public EditorTile(Vector2Int pos)
        {
            Position = pos;
            isOcupied = false;
            isActive = false;
            creatorOcupying = null;
        }
    }
    public EditorBoard(int maxtileWidth, int maxtileHeight, Vector2Int maxactiveTiles, List<PieceCreator> piecesToSpawn, int startingteam, TeamClass.directions[] dirs, bool[] arecompus )
    {
        maxTileWidth = maxtileWidth; maxTileHeight = maxtileHeight; maxActiveTiles = maxactiveTiles; startingTeam = startingteam;
        PiecesToSpawn = new List<PieceCreator>();
        for (int i = 0; i < piecesToSpawn.Count; i++)
        {
            PiecesToSpawn.Add(new PieceCreator(piecesToSpawn[i].team, piecesToSpawn[i].Position, piecesToSpawn[i].type));
        }
        teamsDirs = new TeamClass.directions[4];
        areComputers = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            teamsDirs[i] = dirs[i];
            areComputers[i] = arecompus[i];
        }
        CreateTiles();
    }
    public void CreateTiles()
    {
        allTiles = new EditorTile[maxTileWidth, maxTileHeight];
        for (int w = 0; w < maxTileWidth; w++)
        {
            for (int h = 0; h < maxTileHeight; h++)
            {
                allTiles[w, h] = new EditorTile(new Vector2Int(w, h));
                if (w > maxActiveTiles.x || h > maxActiveTiles.y)
                {
                    allTiles[w, h].isActive = false;
                }
                else
                {
                    allTiles[w, h].isActive = true;
                }
            }
        }
        UpdateActiveTiles(maxActiveTiles);
    }
    public void UpdateActiveTiles(Vector2Int maxActive)
    {
        int activatedTiles = 0;
        for (int w = 0; w < maxTileWidth; w++)
        {
            for (int h = 0; h < maxTileHeight; h++)
            {
                if (w > maxActive.x || h > maxActive.y)
                {
                    allTiles[w, h].isActive = false;
                    activatedTiles++;
                }
                else
                {
                    allTiles[w, h].isActive = true;
                }
            }
        }
        maxActiveTiles = maxActive;
        Debug.Log("updated active tiles: " + activatedTiles);
        assignPiecesInBoardAlready();
    }
    public void assignPiecesInBoardAlready()
    {
        for (int i = 0; i < PiecesToSpawn.Count; i++)
        {
            EditorTile thistile = allTiles[PiecesToSpawn[i].Position.x, PiecesToSpawn[i].Position.y];
            thistile.isOcupied = true;
            thistile.creatorOcupying = PiecesToSpawn[i];
        }
    }
    public void tryAddNewPiece(PieceCreator creator)
    {
        EditorTile thisTile = allTiles[creator.Position.x, creator.Position.y];
        if (thisTile.isOcupied)
        {
            PiecesToSpawn.Remove(thisTile.creatorOcupying);
        }
        thisTile.creatorOcupying = creator;
        thisTile.isOcupied = true;
        PiecesToSpawn.Add(creator);
    }
    public void DeletePieceAtPosition(Vector2Int position)
    {
        EditorTile thisTile = allTiles[position.x, position.y];
        if (!thisTile.isOcupied) { return; }
        thisTile.isOcupied = false;
        PiecesToSpawn.Remove(thisTile.creatorOcupying);
        thisTile.creatorOcupying = null;
    }
    public void RestartBoard()
    {
        CreateTiles();
        PiecesToSpawn.Clear();
    }
    public bool isCreatorPosible(PieceCreator creator)
    {
        EditorTile creatureTile = allTiles[creator.Position.x, creator.Position.y];
        return creatureTile.isActive;
    }
    
}
