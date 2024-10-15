using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caball : Piece
{
    public Caball(Board board, int team, Vector2Int position) : base(board, team, position)
    {

    }
    public override Tile[] GetMovableTiles(Vector2Int startingPos)
    {
        List<Tile> validTiles = new List<Tile>();
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
            if (isVector2Valid(startingPos + VectorsToCheck[i]))
            {
                Tile thisTile = ownBoard.AllTiles[startingPos.x + VectorsToCheck[i].x, startingPos.y + VectorsToCheck[i].y];
                if (!thisTile.isFree && thisTile.currentPiece.Team == Team) { continue; }
                validTiles.Add(thisTile);
            }
        }
        return validTiles.ToArray();
    }
}
