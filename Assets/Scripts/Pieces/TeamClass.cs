using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[Serializable]
public class TeamClass
{
    public string TeamName;
    public List<Piece> piecesList = new List<Piece>();
    public Color PiecesColor;
    public directions directionEnum;
    public Vector2Int directionVector; 
    public bool isDefeated;
    public bool isComputer;
    //public Piece King;
    public int KingIndex;

    public enum directions
    {
        up,down,left,right
    }
    
    public TeamClass(string name, Color color, bool isdefeated, directions dir)
    {
        TeamName = name;
        PiecesColor = color;
        piecesList = new List<Piece>();
        KingIndex = -1;
        isDefeated = isdefeated;
        directionVector = enumToVector(dir);
        directionEnum = dir;
    }
    public void FillTeamWithEnemies(List<Piece> pieces, Board board)
    {
        piecesList.Clear();
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].pieceEnum == Piece.PiecesEnum.Rei)
            {
                if (KingIndex == -1)
                {
                    KingIndex = i;
                }
                else
                {
                    Debug.LogWarning("Two Kings in same team???");
                    continue;
                }
            }
            StaticMethods.CreatePieceByType(pieces[i].pieceEnum, board, pieces[i].Team, pieces[i].Position, pieces[i].hasMoved);
        }
    }
    public void OnDefeated()
    {
        foreach (Piece piece in piecesList)
        {
            piece.isDefeated = true;
        }
        isDefeated = true;
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
