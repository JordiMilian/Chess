using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reina : Piece
{
    public Reina(Board board, int team, Vector2Int position) : base(board, team, position)
    {

    }
    public override Tile[] GetMovableTiles(Vector2Int startingPos)
    {
        List<Tile> validTiles = new List<Tile>();

        int distanceToRight = ownBoard.Width - 1 - startingPos.x;
        int distanceToLeft = startingPos.x;
        int distanceToUp = ownBoard.Height - 1 - startingPos.y;
        int distanceToDown = startingPos.y;

        #region Diagonal
        for (int i = 0; i < Mathf.Min(distanceToUp, distanceToRight); i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x + i + 1, startingPos.y + i + 1];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team)
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
        #endregion
        #region Straight
        for (int i = 0; i < distanceToUp; i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x, startingPos.y + (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile);continue; }
            else if(thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        for (int i = 0; i < distanceToDown; i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x, startingPos.y - (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        for (int i = 0; i < distanceToRight; i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x + (i+1), startingPos.y];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        for (int i = 0; i < distanceToLeft; i++)
        {
            Tile thisTile = ownBoard.AllTiles[startingPos.x - (i +1), startingPos.y];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        #endregion

        return validTiles.ToArray();
    }
}
