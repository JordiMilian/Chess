using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torre : Piece
{
    public Torre(Board board, int team, Vector2Int position) : base(board, team, position)
    {

    }
    public override Tile[] GetMovableTiles(Vector2Int startingPos)
    {
        List<Tile> validTiles = new List<Tile>();

        for (int h = startingPos.y +1; h < ownBoard.Height; h++)
        {
            Tile thisHeight = ownBoard.AllTiles[startingPos.x, h];
            if (thisHeight.isFree) { validTiles.Add(thisHeight); }
            else
            {
                if(thisHeight.currentPiece.Team == Team) { break; }
                else
                {
                    validTiles.Add(thisHeight);
                    break;
                }
            }
        }
        for(int h = startingPos.y -1; h >= 0; h--)
        {
            Tile thisHeight = ownBoard.AllTiles[startingPos.x, h];
            if (thisHeight.isFree) { validTiles.Add(thisHeight); }
            else
            {
                if (thisHeight.currentPiece.Team == Team) { break; }
                else
                {
                    validTiles.Add(thisHeight);
                    break;
                }
            }
        }
        for (int w = startingPos.x + 1; w < ownBoard.Width; w++)
        {
            Tile thisWidth = ownBoard.AllTiles[w, startingPos.y];
            if (thisWidth.isFree) { validTiles.Add(thisWidth); }
            else
            {
                if(thisWidth.currentPiece.Team == Team) { break; }
                else
                {
                    validTiles.Add(thisWidth);
                    break;
                }
            }
        }
        for (int w = startingPos.x -1; w >= 0; w--)
        {
            Tile thisWidth = ownBoard.AllTiles[w, startingPos.y];
            if (thisWidth.isFree) { validTiles.Add(thisWidth); }
            else
            {
                if (thisWidth.currentPiece.Team == Team) { break; }
                else
                {
                    validTiles.Add(thisWidth);
                    break;
                }
            }
        }

        return validTiles.ToArray();
    }
}
