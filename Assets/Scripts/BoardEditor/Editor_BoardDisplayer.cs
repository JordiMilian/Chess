using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecesInstantiator;

public class Editor_BoardDisplayer : MonoBehaviour
{
    [SerializeField] Editor_Controller editorController;
    [SerializeField] Transform startingPosTf;
    [SerializeField] GameObject EditorTilePrefab;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] GameObject prefab_Peo, prefab_Torre, prefab_Caball, prefab_Alfil, prefab_Reina, prefab_Rei;
    [SerializeField] GameObject EditorUIRoot;
    [SerializeField] GameObject EditorBoardRoot;
    Editor_Tile_monobehaviour[,] editorTileMonos = new Editor_Tile_monobehaviour[0,0];
    List<GameObject> InstantiatedEditorPieces = new List<GameObject>();


    public void CreateTilesPrefabs(EditorBoard editorBoard)
    {
        destroyEditorTiles(editorBoard);
        editorTileMonos = new Editor_Tile_monobehaviour[editorBoard.maxTileWidth, editorBoard.maxTileHeight];
        Vector2 currentPos = startingPosTf.position;

        for (int w = 0; w < editorBoard.maxTileWidth; w++)
        {
            for (int h = 0; h < editorBoard.maxTileHeight; h++)
            {
                GameObject editorTileInstance = Instantiate(EditorTilePrefab, currentPos, Quaternion.identity, EditorBoardRoot.transform);
                Editor_Tile_monobehaviour thisEditorTile = editorTileInstance.GetComponent<Editor_Tile_monobehaviour>();
                editorTileMonos[w, h] = thisEditorTile;
                thisEditorTile.OnGotLeftClicked += editorController.OnTileLeftClicked;
                thisEditorTile.OnGotRightClicked += editorController.OnTileRightClicked;
                thisEditorTile.thisEditorTile = editorBoard.allTiles[w, h];

                currentPos.y += distanceBetweenTiles;
            }
            currentPos.x += distanceBetweenTiles;
            currentPos.y = startingPosTf.position.y;
        }
        UpdateTilesDisplay(editorBoard);
        UpdatePiecesDisplay(editorBoard);

        //OnUpdatedHeldPiece?.Invoke(heldPiece);
        //OnUpdatedHeldTeam?.Invoke(heldTeam);
    }
    public void UpdateTilesDisplay(EditorBoard editorBoard)
    {
        int activetile = 0;
        for (int w = 0; w < editorBoard.maxTileWidth; w++)
        {
            for (int h = 0; h < editorBoard.maxTileHeight; h++)
            {
                if (editorBoard.allTiles[w, h].isActive)
                {
                    editorTileMonos[w, h].OnTileActivated();
                    activetile++;
                }
                else
                {
                    editorTileMonos[w, h].OnTileUnactivated();
                }
            }
        }
    }
    public void UpdatePiecesDisplay(EditorBoard MainEditorBoard)
    {
        DestroyInstantiatedPieces();
        for (int i = 0; i < MainEditorBoard.PiecesToSpawn.Count; i++)
        {
            Editor_Controller.PieceCreator thisCreator = MainEditorBoard.PiecesToSpawn[i];
            GameObject instantiatedPiece = Instantiate(
                typeToPrefab(thisCreator.type),
                editorTileMonos[thisCreator.Position.x, thisCreator.Position.y].transform.position,
                Quaternion.identity,
                EditorBoardRoot.transform
                );
            InstantiatedEditorPieces.Add(instantiatedPiece);
            Piece_monobehaviour thisPieceMono = instantiatedPiece.GetComponent<Piece_monobehaviour>();

            if (!MainEditorBoard.isCreatorPosible(thisCreator))
            {
                thisPieceMono.OnDefeated();
            }
            else
            {
                Color pieceColor = editorController.startingTeams[thisCreator.team].PiecesColor;
                thisPieceMono.SetBaseColor(pieceColor);
                thisPieceMono.OnUnselectable();
            }
        }
        GameObject typeToPrefab(Piece.PiecesEnum enume)
        {
            switch (enume)
            {
                case Piece.PiecesEnum.Peo: return prefab_Peo;
                case Piece.PiecesEnum.Torre: return prefab_Torre;
                case Piece.PiecesEnum.Caball: return prefab_Caball;
                case Piece.PiecesEnum.Alfil: return prefab_Alfil;
                case Piece.PiecesEnum.Reina: return prefab_Reina;
                case Piece.PiecesEnum.Rei: return prefab_Rei;
            }
            return null;
        }
        void DestroyInstantiatedPieces()
        {
            for (int i = InstantiatedEditorPieces.Count - 1; i >= 0; i--)
            {
                Destroy(InstantiatedEditorPieces[i]);
            }
            InstantiatedEditorPieces.Clear();
        }
    }
    
    public void destroyEditorTiles(EditorBoard MainEditorBoard) 
    {
        for (int w = editorTileMonos.GetLength(0) - 1; w >= 0; w--)
        {
            for (int h = editorTileMonos.GetLength(1) - 1; h >= 0; h--)
            {
                editorTileMonos[w, h].OnGotLeftClicked -= editorController.OnTileLeftClicked;
                editorTileMonos[w, h].OnGotRightClicked -= editorController.OnTileRightClicked;
                Destroy(editorTileMonos[w, h].gameObject);
            }
        }
        editorTileMonos = new Editor_Tile_monobehaviour[0, 0];
    }
    public void DisableAllDisplays()
    {
        EditorUIRoot.SetActive(false);
        EditorBoardRoot.SetActive(false);
    }
    public void EnableAllDisaplays()
    {
        EditorUIRoot.SetActive(true);
        EditorBoardRoot.SetActive(true);
    }
}
