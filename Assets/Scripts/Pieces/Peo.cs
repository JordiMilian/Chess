using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Board;
using static Piece;

public class Peo : Piece
{
    public Peo(Board board, int team, Vector2Int position, PiecesEnum enume) : base(board, team, position, enume)
    {

    }
    bool hasMoved;
    [SerializeField] GameObject QueenPrefab;


    private void OnEnable()
    {
        onMovedPiece += onMoved;
    }
    private void OnDisable()
    {
        onMovedPiece -= onMoved;
    }
    void onMoved()
    {
        hasMoved = true;
        //If reached end turn into queen
        if (
            currentTile.Coordinates.y == ownBoard.Height - 1 && ownBoard.AllTeams[Team].Direction == 1 || 
            currentTile.Coordinates.y == 0 && ownBoard.AllTeams[Team].Direction == -1)
        {
            //Logica per crear una reina, retocar o algo
            //PiecesInstantiator instantiator = ownBoard.GetComponent<PiecesInstantiator>();
            //instantiator.CreateNewPiece(currentTile, QueenPrefab, Team);
            //onGettingEaten();
        }
    }
    public override Movement[] GetAllPosibleMovements(Vector2Int startingPos)
    {
        bool checkIfTwoSteps = true;

        int negativeMultiplier = ownBoard.AllTeams[Team].Direction;
 

        List<Tile> validTiles = new List<Tile>();

        
        if ((startingPos.y == ownBoard.Height - 1 && negativeMultiplier == 1) || (startingPos.y == 0 && negativeMultiplier == -1))//Tecnicament aixo es imposible perque seria reina
        {
            return new Movement[0];
        }

        // -- ONE STEP FORWARD --
        Tile tileInfront = ownBoard.AllTiles[startingPos.x, startingPos.y + (1 * negativeMultiplier)];
        if (tileInfront.isFree)
        {
            Movement onestepMov = new Movement(Position, tileInfront.Coordinates, Team);
            if(onestepMov.isMoveSaveFromCheck(ownBoard))
            {
                validTiles.Add(tileInfront);
            }
        }
        else
        {
            checkIfTwoSteps = false;
        }

        //  -- TWO STEPS FORWARD --
        if (hasMoved) { checkIfTwoSteps = false; }
        if (checkIfTwoSteps)
        {
            if ((startingPos.y != ownBoard.Height - 2 && negativeMultiplier == 1) || (startingPos.y != 1 && negativeMultiplier == -1)) //If not too near end
            {
                Tile tileTwoInFront = ownBoard.AllTiles[startingPos.x, startingPos.y + (2 * negativeMultiplier)];
                if (tileTwoInFront.isFree)
                {
                    validTiles.Add(tileTwoInFront);
                }
            }
        }

        //   --  DIAGONALS --
        bool checkRight = true, checkLeft = true;

        if (startingPos.x == 0) { checkLeft = false; }
        if (startingPos.x == ownBoard.Width - 1) { checkRight = false; }

        if (checkRight)
        {
            Tile tileTopRight = ownBoard.AllTiles[startingPos.x + 1, startingPos.y + (1 * negativeMultiplier)];
            if (!tileTopRight.isFree && tileTopRight.currentPiece.Team != Team)
            {
                validTiles.Add(tileTopRight);
            }
        }
        if (checkLeft)
        {
            Tile tileTopLeft = ownBoard.AllTiles[startingPos.x - 1, startingPos.y + (1 * negativeMultiplier)];
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
        List<Tile> dangerousTiles = new List<Tile>();
        int negativeMultiplier = ownBoard.AllTeams[Team].Direction;
        bool checkRight = true, checkLeft = true;

        if (Position.x == 0) { checkLeft = false; }
        if (Position.x == ownBoard.Width - 1) { checkRight = false; }

        if (checkRight)
        {
            Tile tileTopRight = ownBoard.AllTiles[Position.x + 1, Position.y + (1 * negativeMultiplier)];
            if (!tileTopRight.isFree && tileTopRight.currentPiece.Team != Team)
            {
                dangerousTiles.Add(tileTopRight);
            }
        }
        if (checkLeft)
        {
            Tile tileTopLeft = ownBoard.AllTiles[Position.x - 1, Position.y + (1 * negativeMultiplier)];
            if (!tileTopLeft.isFree && tileTopLeft.currentPiece.Team != Team)
            {
                dangerousTiles.Add(tileTopLeft);
            }
        }

        return dangerousTiles.ToArray();
    }
}
