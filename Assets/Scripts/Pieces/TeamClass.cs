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
    public int Direction = 1;
    public bool isDefeated;
    public Piece King;

    public TeamClass(string name, Color color, int direction, List<Piece> pieces)
    {
        TeamName = name;
        PiecesColor = color;
        Direction = direction;
        piecesList = new List<Piece>();

    }
    public void FillTeamWithEnemies(List<Piece> pieces, Board board)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].pieceEnum == Piece.PiecesEnum.Rei)
            {
                if (King == null)
                {
                    King = pieces[i];
                }
                else
                {
                    continue;
                }
            }
            piecesList.Add(PiecesInstantiator.GetPieceByType(pieces[i].pieceEnum, board, pieces[i].Team, pieces[i].Position));
        }
    }
}
