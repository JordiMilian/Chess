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
            posibleMoves.Add(newMove);
        }

        if (hasMoved) { return posibleMoves.ToArray(); }

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left,Vector2Int.right,
        };
        foreach (Vector2Int direction in directions)
        {
            Vector2Int positionIn3 = Position + (direction * 3);
            Vector2Int positionIn4 = Position + (direction * 4);
            if(StaticMethods.isVector2inBoard(positionIn3, new Vector2Int(ownBoard.Width -1, ownBoard.Height -1)))
            {
                Tile[] tilesInBetween = new Tile[]
                {
                    ownBoard.AllTiles[(Position + direction * 1).x, (Position + direction * 1).y],
                    ownBoard.AllTiles[(Position + direction * 2).x, (Position + direction * 2).y],
                };
                Tile torreTile = ownBoard.AllTiles[positionIn3.x, positionIn3.y];

                if (!torreTile.isFree 
                    && torreTile.currentPiece.pieceEnum == PiecesEnum.Torre
                    && !torreTile.currentPiece.hasMoved
                    && torreTile.currentPiece.Team == Team
                    && tilesInBetween[0].isFree
                    && tilesInBetween[1].isFree)
                {
                    posibleMoves.Add(new Movement(Position, Position + (direction*2), Team, true, torreTile.Coordinates, tilesInBetween[0].Coordinates));
                }
            }
            if (StaticMethods.isVector2inBoard(positionIn4, new Vector2Int(ownBoard.Width - 1, ownBoard.Height - 1)))
            {
                Tile[] tilesInBetween = new Tile[]
                {
                    ownBoard.AllTiles[(Position + direction * 1).x, (Position + direction * 1).y],
                    ownBoard.AllTiles[(Position + direction * 2).x, (Position + direction * 2).y],
                    ownBoard.AllTiles[(Position + direction * 3).x, (Position + direction * 3).y]
                };

                Tile torreTile = ownBoard.AllTiles[positionIn4.x, positionIn4.y];

                if (!torreTile.isFree
                    && torreTile.currentPiece.pieceEnum == PiecesEnum.Torre
                    && !torreTile.currentPiece.hasMoved
                    && torreTile.currentPiece.Team == Team
                    && tilesInBetween[0].isFree
                    && tilesInBetween[1].isFree
                    && tilesInBetween[2].isFree)
                {
                    posibleMoves.Add(new Movement(Position, Position + (direction * 2), Team, true, torreTile.Coordinates, tilesInBetween[0].Coordinates));
                }
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
