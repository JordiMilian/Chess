using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class Alfil : Piece
{
    public Alfil(Board board, int team, Vector2Int position, PiecesEnum enume, bool isdefeated, bool hasmoved) : base(board, team, position, enume, isdefeated, hasmoved)
    {

    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {

        Tile[] dangerousTiles = GetDangerousTiles();
        List<Movement> posibleMoves = new List<Movement>();
        foreach (Tile tile in dangerousTiles)
        {
            Movement newMove = new Movement(Position, tile.Coordinates, Team);
            posibleMoves.Add(newMove);
        }
        return posibleMoves.ToArray();
    }
    public override Tile[] GetDangerousTiles()
    {
        if (isDefeated) { return new Tile[0]; }

        List<Tile> validTiles = new List<Tile>();

        int distanceToRight = ownBoard.Width - 1 - Position.x;
        int distanceToLeft = Position.x;
        int distanceToUp = ownBoard.Height - 1 - Position.y;
        int distanceToDown = Position.y;
        //TOP RIGHT
        for (int i = 0; i < Mathf.Min(distanceToUp, distanceToRight); i++)
        {
            Tile thisTile = ownBoard.AllTiles[Position.x + i + 1, Position.y + i + 1];
            if (thisTile.isFree) {validTiles.Add(thisTile); continue; }
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

        return validTiles.ToArray();
    }
}
