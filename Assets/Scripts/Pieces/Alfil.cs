using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alfil : Piece
{
    public Alfil(Board board, int team, Vector2Int position) : base(board, team, position)
    {

    }
    public override Tile[] GetMovableTiles(Vector2Int startingPos)
    {
        List<Tile> validTiles = new List<Tile>();

        int distanceToRight = ownBoard.Width -1 - startingPos.x;
        int distanceToLeft = startingPos.x;
        int distanceToUp = ownBoard.Height -1 - startingPos.y;
        int distanceToDown =startingPos.y ;
        /*
        Debug.Log(
            "Distance Right: " + distanceToRight +
            " Up: " + distanceToUp +
            " Left: " + distanceToLeft +
            " Down: " + distanceToDown
            );
        */
        //TOP RIGHT
        for (int i = 0; i < Mathf.Min(distanceToUp,distanceToRight); i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x + i + 1, startingPos.y + i + 1];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if(thisTile.currentPiece.Team != Team)
            {
                validTiles.Add(thisTile);
                break;
            }
            else { break; }
        }
        //TOP LEFT
        for (int i = 0; i < Mathf.Min(distanceToUp, distanceToLeft); i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x - (i + 1), startingPos.y + (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team)
            {
                validTiles.Add(thisTile);
                break;
            }
            else { break; }
        }
        //BOTTOM RIGHT
        for (int i = 0; i < Mathf.Min(distanceToDown, distanceToRight); i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x + i + 1, startingPos.y - (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team)
            {
                validTiles.Add(thisTile);
                break;
            }
            else { break; }
        }
        //BOTTOM LEFT
        for (int i = 0; i < Mathf.Min(distanceToDown, distanceToLeft); i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x - (i + 1), startingPos.y - (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team)
            {
                validTiles.Add(thisTile);
                break;
            }
            else { break; }
        }

        return validTiles.ToArray();
    }
}
