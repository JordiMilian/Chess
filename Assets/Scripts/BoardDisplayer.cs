using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDisplayer : MonoBehaviour
{
    //Board currentBoard; //WRONG WE SHOULD SAVE A NEW BOARD, GET REFERENCE IN METHOD PLS
    [SerializeField] GameController gameController;
    [SerializeField] Transform startingTf;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] GameObject TilePrefab;
    [SerializeField] GameObject BasePiecePrefab;
    [SerializeField] Sprite sprite_peo, sprite_torre, sprite_caball, sprite_alfil, sprite_reina, sprite_rei;
    GameObject[,] tilesInstances = new GameObject[0,0];
    List<GameObject> piecesInstances = new List<GameObject>();
    public void DisplayBoard(Board board)
    {
        Vector2 nextPos = startingTf.position;
        tilesInstances = new GameObject[board.Width, board.Height];

        for (int w = 0; w < board.Width; w++)
        {
            for (int h = 0; h < board.Height; h++)
            {
                GameObject thisTileGO = Instantiate(TilePrefab, nextPos, Quaternion.identity, transform);
                TileMonobehaviour tileMono = thisTileGO.GetComponent<TileMonobehaviour>();
                tileMono.tileScript = board.AllTiles[w, h];
                tileMono.onTileClicked += gameController.TileClicked;
                tilesInstances[w, h] = thisTileGO;
                nextPos.y += distanceBetweenTiles;
            }
            nextPos.x += distanceBetweenTiles;
            nextPos.y = startingTf.position.y;
        }
        UpdatePieces(board);
    }
    public void UpdatePieces(Board board)
    {
        for (int i = piecesInstances.Count -1; i >= 0; i--)
        {
            Destroy(piecesInstances[i]);
        }
        piecesInstances.Clear();

        for (int t = 0; t < board.AllTeams.Count; t++)
        {
            for (int p = 0; p < board.AllTeams[t].piecesList.Count; p++)
            {
                Piece thisPiece = board.AllTeams[t].piecesList[p];
                GameObject newPiece = Instantiate(BasePiecePrefab, tilesInstances[thisPiece.Position.x, thisPiece.Position.y].transform.position, Quaternion.identity);
                
                //Logica per cambiar el sprite, el color del sprite i la opacitat
                SpriteRenderer pieceRenderer = newPiece.GetComponent<SpriteRenderer>();
                pieceRenderer.sprite = GetSpriteByType(thisPiece);
                if (thisPiece.isDefeated) { pieceRenderer.color = Color.black; }
                else { pieceRenderer.color = board.AllTeams[thisPiece.Team].PiecesColor; }
                
                piecesInstances.Add(newPiece);
            }
        }
    }
    public void UpdateHighlighted(Board board)
    {
        for (int w = 0; w < board.Width; w++)
        {
            for (int h = 0; h < board.Height; h++)
            {
                TileMonobehaviour tileMono = tilesInstances[w,h].GetComponent<TileMonobehaviour>();
                tileMono.OnUnhightlight();
                Tile thisTile = board.AllTiles[w, h];
                if(thisTile.isHighlighted)
                {
                    tileMono.OnHighlight();
                    //Logic to highlight tile
                }
            }
        }
    }
    Sprite GetSpriteByType(Piece piece)
    {
        if (piece is Peo) { return sprite_peo; }
        else if (piece is Torre) { return sprite_torre; }
        else if (piece is Caball) { return sprite_caball; }
        else if (piece is Alfil) { return sprite_alfil; }
        else if (piece is Reina) { return sprite_reina; }
        else if (piece is Rei) { return sprite_rei; }
        else return null;
    }
}
