using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Peo : Piece
{
    public Peo(Board board, int team, Vector2Int position) : base(board, team, position)
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
    public override Tile[] GetMovableTiles(Vector2Int startingPos)
    {
        bool checkIfTwoSteps = true;

        int negativeMultiplier = ownBoard.AllTeams[Team].Direction;
 

        List<Tile> validTiles = new List<Tile>();

        
        if ((startingPos.y == ownBoard.Height - 1 && negativeMultiplier == 1) || (startingPos.y == 0 && negativeMultiplier == -1))//Tecnicament aixo es imposible perque seria reina
        {
            return new Tile[0];
        }

        // -- ONE STEP FORWARD --
        Tile tileInfront = ownBoard.AllTiles[startingPos.x, startingPos.y + (1 * negativeMultiplier)];
        if (tileInfront.isFree)
        {
            validTiles.Add(tileInfront);
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
        return validTiles.ToArray();
    }
}
