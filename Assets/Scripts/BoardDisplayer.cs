using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDisplayer : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] Transform startingTf;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] GameObject TilePrefab;
    [SerializeField] Color boardColor01, boardColor02;
    [SerializeField] GameObject prefab_peo, prefab_torre, prefab_caball, Prefab_alfil, prefab_reina, prefab_rei;
    [SerializeField] Transform BoardRootTf, PiecesRootTf, UIRootTf, SquareBG, SquareBorder;
    [SerializeField] BoardDisplaySizeController sizeController;
    GameObject[,] tilesInstances = new GameObject[0,0];
    List<Piece_monobehaviour> piecesInstances = new List<Piece_monobehaviour>();
    [SerializeField] float delayBetweenTiles, delayBetweenPieces;
    [Header("Audio")]
    [SerializeField] AudioClip SetupPieceClip;
    [SerializeField] AudioClip SetupTileClip;
    [SerializeField] int playSoundEachPiece, playSoundEachTile;
    public void DisplayBoard(Board board)
    {
        sizeController.GetBasicSize();
        ShowPlayingStuff();
        Vector2 nextPos = startingTf.position;
        tilesInstances = new GameObject[board.Width, board.Height];

        for (int w = 0; w < board.Width; w++)
        {
            for (int h = 0; h < board.Height; h++)
            {
                GameObject thisTileGO = Instantiate(TilePrefab, nextPos, Quaternion.identity, BoardRootTf);
                TileMonobehaviour tileMono = thisTileGO.GetComponent<TileMonobehaviour>();
                tileMono.tileScript = board.AllTiles[w, h];
                tileMono.onTileClicked += gameController.TileClicked;
                tilesInstances[w, h] = thisTileGO;
                if ((h + w) % 2 != 0) { tileMono.SetBaseColor(boardColor01);}
                else { tileMono.SetBaseColor(boardColor02); }
                tileMono.OnHidden();


                nextPos.y += distanceBetweenTiles;
            }
            nextPos.x += distanceBetweenTiles;
            nextPos.y = startingTf.position.y;
        }
        sizeController.SetSize(board);
        CreateHiddenPieces();
       
    }
    public IEnumerator appearTilesCutscene()
    {
        int tilesCounter = 0;
        for (int w = 0; w < tilesInstances.GetLength(0); w++)
        {
            for (int h = 0; h < tilesInstances.GetLength(1); h++)
            { 
                TileMonobehaviour tileMono = tilesInstances[w, h].GetComponent<TileMonobehaviour>();
                tileMono.OnUnhightlight();
                tileMono.OnAppear();

                tilesCounter++;
                if(tilesCounter > playSoundEachTile)
                {
                    tilesCounter = 0;
                    SFX_PlayerSingleton.Instance.playSFX(SetupTileClip, 0.2f);
                }


                yield return new WaitForSeconds(delayBetweenTiles);
            }
        }
        StartCoroutine(AppearPiecesCutscene());
        
    }
    void CreateHiddenPieces()
    {
        Board board = gameController.GameBoard;
        for (int i = piecesInstances.Count - 1; i >= 0; i--)
        {
            Destroy(piecesInstances[i].gameObject);
        }
        piecesInstances.Clear();

        for (int t = 0; t < board.AllTeams.Count; t++)
        {
            for (int p = 0; p < board.AllTeams[t].piecesList.Count; p++)
            {
                Piece thisPiece = board.AllTeams[t].piecesList[p];
                Piece_monobehaviour pieceMono = Instantiate(
                    GetPrefabByType(board.AllTeams[t].piecesList[p]),
                    tilesInstances[thisPiece.Position.x, thisPiece.Position.y].transform.position,
                    Quaternion.identity,
                    PiecesRootTf
                    ).GetComponent<Piece_monobehaviour>();
                pieceMono.pieceScript = thisPiece;
                pieceMono.SetBaseColor(board.AllTeams[thisPiece.Team].PiecesColor);

                pieceMono.OnHidden();

                piecesInstances.Add(pieceMono);
            }
        }
    }
    public IEnumerator AppearPiecesCutscene()
    {
        int piecesCounter = 0;
        Board board = gameController.GameBoard;
        for (int i = 0; i < piecesInstances.Count; i++)
        {
            piecesInstances[i].OnAppeared();

            piecesCounter++;
            if(piecesCounter > playSoundEachPiece)
            {
                piecesCounter = 0;
                SFX_PlayerSingleton.Instance.playSFX(SetupPieceClip, 0.2f);
                Debug.Log("played piece setup");
            }
            yield return new WaitForSeconds(delayBetweenPieces);
        }
        UpdatePieces(gameController.GameBoard, null);
    }
    public void UpdatePieces(Board board, Piece movedPiece)
    {
        for (int i = piecesInstances.Count -1; i >= 0; i--)
        {
            Destroy(piecesInstances[i].gameObject);
        }
        piecesInstances.Clear();

        for (int t = 0; t < board.AllTeams.Count; t++)
        {
            for (int p = 0; p < board.AllTeams[t].piecesList.Count; p++)
            {
                Piece thisPiece = board.AllTeams[t].piecesList[p];
                Piece_monobehaviour pieceMono = Instantiate(
                    GetPrefabByType(board.AllTeams[t].piecesList[p]), 
                    tilesInstances[thisPiece.Position.x, thisPiece.Position.y].transform.position, 
                    Quaternion.identity,
                    PiecesRootTf
                    ).GetComponent<Piece_monobehaviour>();
                pieceMono.pieceScript = thisPiece;


                pieceMono.SetBaseColor( board.AllTeams[thisPiece.Team].PiecesColor);

                if (thisPiece.isDefeated) { pieceMono.OnDefeated(); }
                else if (thisPiece is Rei && gameController.GameBoard.isPlayerInCheck(t))
                {
                    pieceMono.OnPermanentlyKillable();
                }
                else if (thisPiece.isSelectable) { pieceMono.OnSelectable(); }
                else { pieceMono.OnUnselectable(); ; }

                if(gameController.GameBoard.lastMovedPiece == thisPiece)
                {
                    pieceMono.OnGotMoved();
                    //gameController.GameBoard.lastMovedPiece = null;
                }
                piecesInstances.Add(pieceMono);
            }
        }
    }
    public void UpdateHighlighted(Board board, Tile selectedTile)
    {
        for (int w = 0; w < board.Width; w++)
        {
            for (int h = 0; h < board.Height; h++)
            {
                TileMonobehaviour tileMono = tilesInstances[w,h].GetComponent<TileMonobehaviour>();
                tileMono.OnUnhightlight();
                Tile thisTile = board.AllTiles[w, h];
                if(thisTile.isLegalTile)
                {
                    tileMono.OnHighlight();
                    //Logic to highlight tile
                }
                else if(thisTile.isPosibleTile)
                {
                    tileMono.OnHighlightedButChecks();
                }
            }
        }
        foreach(Piece_monobehaviour piece in piecesInstances)
        {
            if(piece.pieceScript.currentTile == selectedTile)
            {
                piece.OnGotSelected();
            }
            else if(piece.pieceScript.currentTile.isLegalTile && !piece.pieceScript.isDefeated)
            {
                piece.OnKillable();
            }
            else
            {
                piece.OnNotKillable();
            }
        }
    }
    GameObject GetPrefabByType(Piece piece)
    {
        if (piece is Peo) { return prefab_peo; }
        else if (piece is Torre) { return prefab_torre; }
        else if (piece is Caball) { return prefab_caball; }
        else if (piece is Alfil) { return Prefab_alfil; }
        else if (piece is Reina) { return prefab_reina; }
        else if (piece is Rei) { return prefab_rei; }
        else return null;
    }
    public void HidePLayingStuff()
    {
        BoardRootTf.gameObject.SetActive(false);
        PiecesRootTf.gameObject.SetActive(false);
        UIRootTf.gameObject.SetActive(false);
        SquareBG.gameObject.SetActive(false);
        SquareBorder.gameObject.SetActive(false);
    }
    public void ShowPlayingStuff()
    {
        BoardRootTf.gameObject.SetActive(true);
        PiecesRootTf.gameObject.SetActive(true);
        UIRootTf.gameObject.SetActive(true);
        SquareBG.gameObject.SetActive(true);
        SquareBorder.gameObject.SetActive(true);
    }
    public void DestroyCurrentBoard()
    {
        for (int w = tilesInstances.GetLength(0) -1; w >= 0; w--)
        {
            for (int h = tilesInstances.GetLength(1) - 1; h >= 0; h--)
            {
                Destroy(tilesInstances[w,h].gameObject);
            }
        }
        for (int p = piecesInstances.Count - 1; p >= 0; p--)
        {
            Destroy(piecesInstances[p].gameObject);
        }
            
    }
}
