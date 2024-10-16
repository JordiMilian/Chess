using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class Caball : Piece
{
    public Caball(Board board, int team, Vector2Int position, PiecesEnum enume) : base(board, team, position, enume)
    {

    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {
        List<Movement> validMoves = new List<Movement>();
        Vector2Int[] VectorsToCheck = new Vector2Int[]
        {
            new Vector2Int(1, 2),
            new Vector2Int(-1, 2),
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, -1),
            new Vector2Int(1, -2),
            new Vector2Int(-1, -2)
        };
        for (int i = 0; i < VectorsToCheck.Length; i++)
        {
            if (isVector2inBoard(startingPos + VectorsToCheck[i]))
            {
                Tile thisTile = ownBoard.AllTiles[startingPos.x + VectorsToCheck[i].x, startingPos.y + VectorsToCheck[i].y];
                if (!thisTile.isFree && thisTile.currentPiece.Team == Team) { continue; }
                Movement newMove = new Movement(Position, startingPos + VectorsToCheck[i], Team);
                if (newMove.isMoveSaveFromCheck(ownBoard))
                {
                    validMoves.Add(newMove);
                }
            }
        }
        return validMoves.ToArray();
    }
    public override Tile[] GetDangerousTiles()
    {
        List<Tile> validMoves = new List<Tile>();
        Vector2Int[] VectorsToCheck = new Vector2Int[]
        {
            new Vector2Int(1, 2),
            new Vector2Int(-1, 2),
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, -1),
            new Vector2Int(1, -2),
            new Vector2Int(-1, -2)
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
