using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Board;
using static TeamClass;

public class Peo : Piece
{
    public bool justDobleMoved;
    public Peo(Board board, int team, Vector2Int position, PiecesEnum enume, bool isdefeated, bool hasmoved) :
        base(board, team, position, enume, isdefeated, hasmoved)
    {
        onMovedPiece += onMoved;
    }

    void onMoved(Movement mov)
    {
        Vector2Int movementDistance = mov.startPos - mov.endPos;
        if(Mathf.Abs(movementDistance.x) >= 2 || Mathf.Abs(movementDistance.y) >= 2) { justDobleMoved = true; Debug.Log("just doble moved"); }

        hasMoved = true;
        //If reached end turn into queen
        if (Position.y == ownBoard.Height - 1 && ownBoard.AllTeams[Team].directionVector == Vector2Int.up ||
            Position.y == 0 && ownBoard.AllTeams[Team].directionVector == Vector2Int.down ||
            Position.x == ownBoard.Width - 1 && ownBoard.AllTeams[Team].directionVector == Vector2Int.right ||
            Position.x == 0 && ownBoard.AllTeams[Team].directionVector == Vector2Int.left 
            )
        {
            onGettingEaten();
            StaticMethods.CreatePieceByType(PiecesEnum.Reina, ownBoard, Team, Position, false);
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
                validTiles.Add(tileInfront);
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
            validMovement.Add(newMove);
        }


        //   --  AN PASANT --

        Vector2Int leftPos = Position + rotateVector(new Vector2Int(-1, 0), ownBoard.AllTeams[Team].directionVector);
        if(isVector2inBoard(leftPos))
        {
            Tile leftTile = ownBoard.AllTiles[leftPos.x, leftPos.y];
            if (!leftTile.isFree && leftTile.currentPiece is Peo)
            {
                Peo peoLeft = (Peo)leftTile.currentPiece;
                if(peoLeft.justDobleMoved)
                {
                    Vector2Int enemyDirection = ownBoard.AllTeams[peoLeft.Team].directionVector;
                    if (enemyDirection == -ownBoard.AllTeams[Team].directionVector)
                    {
                        validMovement.Add(new Movement(Position, leftPos, Team, true, leftPos, topLeftPos));
                    }
                }
            }
        }
        Vector2Int rightPos = Position + rotateVector(new Vector2Int(1, 0), ownBoard.AllTeams[Team].directionVector);
        if (isVector2inBoard(rightPos))
        {
            Tile rightTile = ownBoard.AllTiles[rightPos.x, rightPos.y];
            if (!rightTile.isFree && rightTile.currentPiece is Peo)
            {
                Peo peoRight = (Peo)rightTile.currentPiece;
                if (peoRight.justDobleMoved)
                {
                    Vector2Int enemyDirection = ownBoard.AllTeams[peoRight.Team].directionVector;
                    if(enemyDirection == -ownBoard.AllTeams[Team].directionVector) 
                    {
                        validMovement.Add(new Movement(Position, rightPos, Team, true, rightPos, topRightpos));
                    }
                    
                }
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
