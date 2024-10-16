using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class Reina : Piece
{
    public Reina(Board board, int team, Vector2Int position, PiecesEnum enume) : base(board, team, position, enume)
    {

    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {
        Tile[] validTiles = GetDangerousTiles();
        List<Movement> posibleMoves = new List<Movement>();
        foreach(Tile tile in validTiles)
        {
            Movement newMove = new Movement(Position, tile.Coordinates, Team);
            if(newMove.isMoveSaveFromCheck(ownBoard))
            {
                posibleMoves.Add(newMove);
            }
        }
        return posibleMoves.ToArray();
    }
    public override Tile[] GetDangerousTiles()
    {
        List<Tile> validTiles = new List<Tile>();

        int distanceToRight = ownBoard.Width - 1 - Position.x;
        int distanceToLeft = Position.x;
        int distanceToUp = ownBoard.Height - 1 - Position.y;
        int distanceToDown = Position.y;

        #region Diagonal
        for (int i = 0; i < Mathf.Min(distanceToUp, distanceToRight); i++)
        {
            Tile thisTile = ownBoard.AllTiles[Position.x + i + 1, Position.y + i + 1];
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
            Tile thisTile = ownBoard.AllTiles[Position.x - (i + 1), Position.y + (i + 1)];
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
            Tile thisTile = ownBoard.AllTiles[Position.x + i + 1, Position.y - (i + 1)];
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
            Tile thisTile = ownBoard.AllTiles[Position.x - (i + 1), Position.y - (i + 1)];
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
            Tile thisTile = ownBoard.AllTiles[Position.x, Position.y + (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        for (int i = 0; i < distanceToDown; i++)
        {   
            Tile thisTile = ownBoard.AllTiles[Position.x, Position.y - (i + 1)];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        for (int i = 0; i < distanceToRight; i++)
        {
            Tile thisTile = ownBoard.AllTiles[Position.x + (i + 1), Position.y];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        for (int i = 0; i < distanceToLeft; i++)
        {
            Tile thisTile = ownBoard.AllTiles[Position.x - (i + 1), Position.y];
            if (thisTile.isFree) { validTiles.Add(thisTile); continue; }
            else if (thisTile.currentPiece.Team != Team) { validTiles.Add(thisTile); break; }
            else { break; }
        }
        #endregion
        return validTiles.ToArray();
    }
}
