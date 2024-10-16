using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Board;

public class Rei : Piece
{
    public Rei(Board board, int team, Vector2Int position, PiecesEnum enume, bool isdefeated, bool hasmoved) : 
        base(board, team, position, enume, isdefeated, hasmoved)
    {

    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {
        Tile[] dangerousTiles = GetDangerousTiles();
        List<Movement> posibleMoves = new List<Movement>();
        foreach (Tile tile in dangerousTiles)
        {
            Movement newMove = new Movement(Position, tile.Coordinates, Team);
            if (newMove.isMoveSaveFromCheck(ownBoard))
            {
                posibleMoves.Add(newMove);
            }
        }
        return posibleMoves.ToArray();
    }
    public override Tile[] GetDangerousTiles()
    {
        if (isDefeated) { return new Tile[0]; }

        List<Tile> validMoves = new List<Tile>();
        Vector2Int[] VectorsToCheck = new Vector2Int[8]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1)
        };
        for (int i = 0; i < VectorsToCheck.Length; i++)
        {
            if (isVector2inBoard(Position + VectorsToCheck[i]))
            {
                Tile thisTile = ownBoard.AllTiles[Position.x + VectorsToCheck[i].x, Position.y + VectorsToCheck[i].y];
                if (!thisTile.isFree && thisTile.currentPiece.Team == Team) { continue; }
                validMoves.Add(thisTile);
            }
        }

        return validMoves.ToArray();
    }
}
