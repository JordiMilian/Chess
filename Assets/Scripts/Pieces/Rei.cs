using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rei : Piece
{
    public Rei (Board board, int team, Vector2Int position) : base(board,team, position)
    {

    }
    public override Tile[] GetMovableTiles(Vector2Int startingPos)
    {
        Tile[] dangerousTiles = ownBoard.GetAllDangerousTiles(Team);

        List<Tile> validTiles = new List<Tile>();
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
            if (isVector2Valid(startingPos + VectorsToCheck[i]))
            {
                Tile thisTile = ownBoard.AllTiles[startingPos.x + VectorsToCheck[i].x, startingPos.y + VectorsToCheck[i].y];
                if (dangerousTiles.Contains(thisTile)) { continue; }
                if(!thisTile.isFree && thisTile.currentPiece.Team == Team) { continue; }
                validTiles.Add(thisTile);
            }
        }

        return validTiles.ToArray();
    }
}
