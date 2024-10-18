using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecesInstantiator;

public class Editor_Controller : MonoBehaviour
{
    
    public EditorBoard MainEditorBoard;
    [SerializeField] PiecesInstantiator piecesInstantiator;
    [SerializeField] GameObject EditorTilePrefab;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] Transform startingPosTf;
    [SerializeField] GameObject prefab_Peo, prefab_Torre, prefab_Caball, prefab_Alfil, prefab_Reina, prefab_Rei;
    public int heldTeam;
    public Piece.PiecesEnum heldPiece;
    Editor_Tile_monobehaviour[,] editorTileMonos;
    List<GameObject> InstantiatedEditorPieces = new List<GameObject>();
    

    private void Awake()
    {
        MainEditorBoard.CreateTiles();
        CreateTilesPrefabs();
    }
    void CreateTilesPrefabs()
    {
        editorTileMonos = new Editor_Tile_monobehaviour[MainEditorBoard.maxTileWidth, MainEditorBoard.maxTileHeight];
        Vector2 currentPos = startingPosTf.position;

        for (int w = 0; w < MainEditorBoard.maxTileWidth; w++)
        {
            for (int h = 0; h < MainEditorBoard.maxTileHeight; h++)
            {
                GameObject editorTileInstance = Instantiate(EditorTilePrefab, currentPos, Quaternion.identity);
                Editor_Tile_monobehaviour thisEditorTile = editorTileInstance.GetComponent<Editor_Tile_monobehaviour>();
                editorTileMonos[w, h] = thisEditorTile;
                thisEditorTile.OnGotLeftClicked += OnTileLeftClicked;
                thisEditorTile.OnGotRightClicked += OnTileRightClicked;
                thisEditorTile.thisEditorTile = MainEditorBoard.allTiles[w, h];

                currentPos.y += distanceBetweenTiles;
            }
            currentPos.x += distanceBetweenTiles;
            currentPos.y = startingPosTf.position.y;
        }
        UpdateTilesDisplay();
    }
    

    //If tile is left clicked we add the Held piece creator into this position
    public void OnTileLeftClicked(EditorBoard.EditorTile tile)
    {
        MainEditorBoard.tryAddNewPiece(new PieceCreator(heldTeam, tile.Position, heldPiece));
        UpdatePiecesDisplay();
    }
    //If tile is right clicked we either delete piece in position or update the board
    public void OnTileRightClicked(EditorBoard.EditorTile tile)
    {
        if(tile.isOcupied)
        {
            MainEditorBoard.DeletePieceAtPosition(tile.Position);
            UpdatePiecesDisplay();
        }
        else
        {
            MainEditorBoard.UpdateActiveTiles(tile.Position);
            UpdateTilesDisplay();
            UpdatePiecesDisplay();
        }
    }

    void UpdateTilesDisplay()
    {
        for (int w = 0; w < MainEditorBoard.maxTileWidth; w++)
        {
            for (int h = 0; h < MainEditorBoard.maxTileHeight; h++)
            {
                if (MainEditorBoard.allTiles[w,h].isActive)
                {
                    editorTileMonos[w, h].OnTileActivated();
                }
                else
                {
                    editorTileMonos[w, h].OnTileUnactivated();
                }
            }
        }
    }
    void UpdatePiecesDisplay()
    {
        DestroyInstantiatedPieces();
        
        for (int i = 0; i < MainEditorBoard.PiecesToSpawn.Count; i++)
        {
            PieceCreator thisCreator = MainEditorBoard.PiecesToSpawn[i];
            GameObject instantiatedPiece = Instantiate(typeToPrefab(
                thisCreator.type),
                editorTileMonos[thisCreator.Position.x, thisCreator.Position.y].transform.position,
                Quaternion.identity
                );
            InstantiatedEditorPieces.Add(instantiatedPiece);
            Piece_monobehaviour thisPieceMono = instantiatedPiece.GetComponent<Piece_monobehaviour>();

            if(!MainEditorBoard.isCreatorPosible(thisCreator))
            {
                thisPieceMono.OnDefeated();
            }
            else
            {
                Color pieceColor = piecesInstantiator.startingBoard.AllTeams[thisCreator.team].PiecesColor;
                thisPieceMono.SetBaseColor(pieceColor);
                thisPieceMono.OnUnselectable();
            }
        }
    }
    void DestroyInstantiatedPieces()
    {
        for (int i = InstantiatedEditorPieces.Count -1; i >= 0; i--)
        {
            Destroy(InstantiatedEditorPieces[i]);
        }
        InstantiatedEditorPieces.Clear();
        
    }
    void destroyEditorTiles() //this is for when the game actually begins
    {
        for (int w = MainEditorBoard.maxTileWidth - 1; w >= 0; w--)
        {
            for (int h = MainEditorBoard.maxTileHeight - 1; h >= 0; h--)
            {
                editorTileMonos[w, h].OnGotLeftClicked -= OnTileLeftClicked;
                editorTileMonos[w, h].OnGotRightClicked -= OnTileRightClicked;
                Destroy(editorTileMonos[w, h].gameObject);
            }
        }
        editorTileMonos = new Editor_Tile_monobehaviour[0, 0];
    }
    public Board EditorToBoard(EditorBoard editBoard)
    {
        return null;
    }
    GameObject typeToPrefab(Piece.PiecesEnum enume)
    {
        switch (enume) {
            case Piece.PiecesEnum.Peo: return prefab_Peo;
            case Piece.PiecesEnum.Torre: return prefab_Torre;
            case Piece.PiecesEnum.Caball: return prefab_Caball;
            case Piece.PiecesEnum.Alfil: return prefab_Alfil;
            case Piece.PiecesEnum.Reina: return prefab_Reina;
            case Piece.PiecesEnum.Rei: return prefab_Rei;
        }
        return null;
    }

}
