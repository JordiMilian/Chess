using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Board;
using static TeamClass;

public class Peo : Piece
{
    public Peo(Board board, int team, Vector2Int position, PiecesEnum enume, bool isdefeated, bool hasmoved) :
        base(board, team, position, enume, isdefeated, hasmoved)
    {
        onMovedPiece += onMoved;
    }

    void onMoved()
    {
        hasMoved = true;
        //If reached end turn into queen
        if (Position.y == ownBoard.Height - 1 && ownBoard.AllTeams[Team].directionVector == Vector2Int.up ||
            Position.y == 0 && ownBoard.AllTeams[Team].directionVector == Vector2Int.down ||
            Position.x == ownBoard.Width - 1 && ownBoard.AllTeams[Team].directionVector == Vector2Int.right ||
            Position.x == 0 && ownBoard.AllTeams[Team].directionVector == Vector2Int.left 
            )
        {
            onGettingEaten();
            Editor_Controller.GetPieceByType(PiecesEnum.Reina, ownBoard, Team, Position, false);
            ownBoard.UpdateKingsIndex();
        }
    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {
        bool checkIfTwoSteps = true;

        List<Tile> validTiles = new List<Tile>();

        // -- ONE STEP FORWARD --
        Vector2Int VectroInFront = Position + rotateVector(Vector2Int.up, ownBoard.AllTeams[Team].directionVector);
        if(isVector2inBoard(VectroInFront))
        {
            Tile tileInfront = ownBoard.AllTiles[VectroInFront.x, VectroInFront.y];
            if (tileInfront.isFree)
            {
                Movement onestepMov = new Movement(Position, tileInfront.Coordinates, Team);
                if (onestepMov.isMoveSaveFromCheck(ownBoard))
                {
                    validTiles.Add(tileInfront);
                }
            }
            else
            {
                checkIfTwoSteps = false;
            }
        }
       
        //  -- TWO STEPS FORWARD --
        if (hasMoved) { checkIfTwoSteps = false; }
        if (checkIfTwoSteps)
        {
            Vector2Int VectroTwoInFront = Position + rotateVector(new Vector2Int(0,2), ownBoard.AllTeams[Team].directionVector);
            if (isVector2inBoard(VectroTwoInFront))
            {
                Tile tileTwoInFront = ownBoard.AllTiles[VectroTwoInFront.x, VectroTwoInFront.y];
                if (tileTwoInFront.isFree)
                {
                    validTiles.Add(tileTwoInFront);
                }
            }
        }

        //   --  DIAGONALS --

        Vector2Int topRightpos = Position + rotateVector(new Vector2Int(1, 1), ownBoard.AllTeams[Team].directionVector);
        if (isVector2inBoard(topRightpos))
        {
            Tile tileTopRight = ownBoard.AllTiles[topRightpos.x, topRightpos.y];
            if (!tileTopRight.isFree && tileTopRight.currentPiece.Team != Team)
            {
                validTiles.Add(tileTopRight);
            }
        }
        Vector2Int topLeftPos = Position + rotateVector(new Vector2Int(-1, 1), ownBoard.AllTeams[Team].directionVector);
        if (isVector2inBoard(topLeftPos))
        {
            Tile tileTopLeft = ownBoard.AllTiles[topLeftPos.x, topLeftPos.y];
            if (!tileTopLeft.isFree && tileTopLeft.currentPiece.Team != Team)
            {
                validTiles.Add(tileTopLeft);
            }
        }


        List<Movement> validMovement = new List<Movement>();
        foreach (Tile tile in validTiles)
        {
            Movement newMove = new Movement(Position, tile.Coordinates, Team);
            if (newMove.isMoveSaveFromCheck(ownBoard))
            {
                validMovement.Add(newMove);
            }
        }
        return validMovement.ToArray();
    }
    public override Tile[] GetDangerousTiles()
    {
        if (isDefeated) { return new Tile[0]; }

        List<Tile> dangerousTiles = new List<Tile>();

        Vector2Int topRightpos = Position + rotateVector(new Vector2Int(1, 1), ownBoard.AllTeams[Team].directionVector);
        if (isVector2inBoard(topRightpos))
        {
            Tile tileTopRight = ownBoard.AllTiles[topRightpos.x, topRightpos.y];
            if (!tileTopRight.isFree && tileTopRight.currentPiece.Team != Team)
            {
                dangerousTiles.Add(tileTopRight);
            }
        }
        Vector2Int topLeftPos = Position + rotateVector(new Vector2Int(-1, 1), ownBoard.AllTeams[Team].directionVector);
        if (isVector2inBoard(topLeftPos))
        {
            Tile tileTopLeft = ownBoard.AllTiles[topLeftPos.x, topLeftPos.y];
            if (!tileTopLeft.isFree && tileTopLeft.currentPiece.Team != Team)
            {
                dangerousTiles.Add(tileTopLeft);
            }
        }
        return dangerousTiles.ToArray();
    }
    Vector2Int rotateVector(Vector2Int vector, Vector2Int newForward )
    {
        if(newForward == Vector2Int.down) { return new Vector2Int(-vector.x, -vector.y); }
        if(newForward == Vector2Int.right) { return new Vector2Int(vector.y, -vector.x); }
        if(newForward == Vector2Int.left) { return new Vector2Int(-vector.y, vector.x); }
        return vector;
    }
}
