using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class Torre : Piece
{
    public Torre(Board board, int team, Vector2Int position, PiecesEnum enume, bool isdefeated, bool hasmoved) : 
        base(board, team, position, enume, isdefeated, hasmoved)
    {
    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {
        List<Movement> validTiles = new List<Movement>();

        for (int h = startingPos.y +1; h < ownBoard.Height; h++)
        {
            Tile thisHeight = ownBoard.AllTiles[startingPos.x, h];
            if (thisHeight.isFree) { AddMoveToList(thisHeight, ref validTiles); }
            else
            {
                if(thisHeight.currentPiece.Team == Team) { break; }
                else
                {
                    AddMoveToList(thisHeight, ref validTiles);
                    break;
                }
            }
        }
        for(int h = startingPos.y -1; h >= 0; h--)
        {
            Tile thisHeight = ownBoard.AllTiles[startingPos.x, h];
            if (thisHeight.isFree) { AddMoveToList(thisHeight, ref validTiles); }
            else
            {
                if (thisHeight.currentPiece.Team == Team) { break; }
                else
                {
                    AddMoveToList(thisHeight, ref validTiles);
                    break;
                }
            }
        }
        for (int w = startingPos.x + 1; w < ownBoard.Width; w++)
        {
            Tile thisWidth = ownBoard.AllTiles[w, startingPos.y];
            if (thisWidth.isFree) { AddMoveToList(thisWidth, ref validTiles); }
            else
            {
                if(thisWidth.currentPiece.Team == Team) { break; }
                else
                {
                    AddMoveToList(thisWidth, ref validTiles);
                    break;
                }
            }
        }
        for (int w = startingPos.x -1; w >= 0; w--)
        {
            Tile thisWidth = ownBoard.AllTiles[w, startingPos.y];
            if (thisWidth.isFree) { AddMoveToList(thisWidth, ref validTiles); }
            else
            {
                if (thisWidth.currentPiece.Team == Team) { break; }
                else
                {
                    AddMoveToList(thisWidth, ref validTiles);
                    break;
                }
            }
        }
        return validTiles.ToArray();
    }
    public override Tile[] GetDangerousTiles()
    {
        if (isDefeated) { return new Tile[0]; }
        List<Tile> validTiles = new List<Tile>();

        for (int h = Position.y + 1; h < ownBoard.Height; h++)
        {
            Tile thisHeight = ownBoard.AllTiles[Position.x, h];
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
        for (int h = Position.y - 1; h >= 0; h--)
        {
            Tile thisHeight = ownBoard.AllTiles[Position.x, h];
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
        for (int w = Position.x + 1; w < ownBoard.Width; w++)
        {
            Tile thisWidth = ownBoard.AllTiles[w, Position.y];
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
        for (int w = Position.x - 1; w >= 0; w--)
        {
            Tile thisWidth = ownBoard.AllTiles[w, Position.y];
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
    void AddMoveToList(Tile tile, ref List<Movement> moves)
    {
        Movement newMove = new Movement(Position, tile.Coordinates, Team);
        moves.Add(newMove);
    }
}
