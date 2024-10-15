using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDisplayer : MonoBehaviour
{
    Board currentBoard; //WRONG WE SHOULD SAVE A NEW BOARD, GET REFERENCE IN METHOD PLS
    [SerializeField] Transform startingTf;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] GameObject TilePrefab;
    [SerializeField] GameObject BasePiecePrefab;
    GameObject[,] tilesInstances = new GameObject[0,0];
    List<GameObject> piecesInstances = new List<GameObject>();
    public void DisplayBoard(Board board)
    {
        currentBoard = board;
        Vector2 nextPos = startingTf.position;
        tilesInstances = new GameObject[board.Width, board.Height];

        for (int w = 0; w < board.Width; w++)
        {
            for (int h = 0; h < board.Height; h++)
            {
                GameObject thisTileGO = Instantiate(TilePrefab, nextPos, Quaternion.identity, transform);
                tilesInstances[w, h] = thisTileGO;
                nextPos.y += distanceBetweenTiles;
            }
            nextPos.x += distanceBetweenTiles;
            nextPos.y = startingTf.position.y;
        }
        UpdatePieces();
    }
    public void UpdatePieces()
    {
        for (int i = piecesInstances.Count -1; i >= 0; i--)
        {
            Destroy(piecesInstances[i]);
        }
        piecesInstances.Clear();

        for (int t = 0; t < currentBoard.AllTeams.Count; t++)
        {
            for (int p = 0; p < currentBoard.AllTeams[t].piecesList.Count; p++)
            {
                Piece thisPiece = currentBoard.AllTeams[t].piecesList[p];
                GameObject newPiece = Instantiate(BasePiecePrefab, tilesInstances[thisPiece.Position.x, thisPiece.Position.y].transform.position, Quaternion.identity);
                //Logica per cambiar el sprite, el color del sprite i la opacitat
                piecesInstances.Add(newPiece);
            }
        }
    }
    public void UpdateHighlighted()
    {
        for (int w = 0; w < currentBoard.Width; w++)
        {
            for (int h = 0; h < currentBoard.Height; h++)
            {
                Tile thisTile = currentBoard.AllTiles[w, h];
                if(thisTile.isHighlighted)
                {
                    //Logic to highlight tile
                }
            }
        }
    }
}
