using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecesInstantiator;

public class Editor_Controller : MonoBehaviour
{
    
    public EditorBoard MainEditorBoard;
    [SerializeField] GameObject EditorTilePrefab;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] Transform startingPosTf;
    [SerializeField] GameObject prefab_Peo, prefab_Torre, prefab_Caball, prefab_Alfil, prefab_Reina, prefab_Rei;
    public int heldTeam;
    public Piece.PiecesEnum heldPiece;
    public List<TeamClass> startingTeams = new List<TeamClass>();
    Editor_Tile_monobehaviour[,] editorTileMonos;
    List<GameObject> InstantiatedEditorPieces = new List<GameObject>();
    public Action<int> OnUpdatedHeldTeam;
    public Action<Piece.PiecesEnum> OnUpdatedHeldPiece;
    //public List<PieceCreator> PiecesToCreate = new List<PieceCreator>();

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
        OnUpdatedHeldPiece?.Invoke(heldPiece);
        OnUpdatedHeldTeam?.Invoke(heldTeam);
    }
    

    //If tile is left clicked we add the Held piece creator into this position
    public void OnTileLeftClicked(EditorBoard.EditorTile tile)
    {
        if(heldPiece == Piece.PiecesEnum.Rei)
        {
            for (int i = 0; i < MainEditorBoard.PiecesToSpawn.Count; i++)
            {
                if (MainEditorBoard.PiecesToSpawn[i].team == heldTeam && MainEditorBoard.PiecesToSpawn[i].type == Piece.PiecesEnum.Rei)
                {
                    Debug.LogWarning(startingTeams[heldTeam].TeamName + " has a King alrady");
                    return;
                }
        }
        }
        
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
                Color pieceColor = startingTeams[thisCreator.team].PiecesColor;
                thisPieceMono.SetBaseColor(pieceColor);
                thisPieceMono.OnUnselectable();
            }
        }
    }
    public void DestroyInstantiatedPieces()
    {
        for (int i = InstantiatedEditorPieces.Count -1; i >= 0; i--)
        {
            Destroy(InstantiatedEditorPieces[i]);
        }
        InstantiatedEditorPieces.Clear();
        
    }
    public void destroyEditorTiles() //this is for when the game actually begins
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
        return new Board(editBoard.maxActiveTiles.y +1,
            editBoard.maxActiveTiles.x+1,
            startingTeams,//starting team is empty, we must create the pieces later
            editBoard.startingTeam,
            true
            ) ;
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
    public void UpdateHeldType(Piece.PiecesEnum pieceType)
    {
        heldPiece = pieceType;
        OnUpdatedHeldPiece?.Invoke(pieceType);
        //update icon
    }
    public void UpdateHeldTeam(int index)
    {
        heldTeam = index;
        OnUpdatedHeldTeam?.Invoke(index);
    }
    public void UpdateDiretion(int teamIndex, TeamClass.directions directionEnum)
    { 
        startingTeams[teamIndex].directionEnum = directionEnum;
    }
    
    public static void CreatePieces(List<PieceCreator> creators, Board board)
    {
        //gameController = GetComponent<GameController>();
        for (int i = 0; i < creators.Count; i++)
        {
            if (!isVector2inBoard(creators[i].Position, new Vector2Int(board.Width-1, board.Height-1)))
            {
                Debug.LogWarning("Piece to create out of bounds");
                continue;
            }
            PieceCreator piece = creators[i];
            if (!board.AllTiles[piece.Position.x, piece.Position.y].isFree)
            {
                Debug.LogWarning("Tried to create piece " + piece.type + " on an not free place");
                continue;
            }
            if (piece.type == Piece.PiecesEnum.Rei)
            {
                if (board.AllTeams[piece.team].KingIndex != -1) { Debug.LogWarning("Tried to create two Kings for the same team"); continue; }
            }

            Piece newPiece = GetPieceByType(piece.type, board, piece.team, piece.Position, false);
            if (piece.type == Piece.PiecesEnum.Rei)
            {
                board.AllTeams[piece.team].KingIndex = i;
            }
        }
        board.UpdateKingsIndex();
    }
    public static Piece GetPieceByType(Piece.PiecesEnum pieceKey, Board board, int team, Vector2Int pos, bool hasmoved)
    {
        switch (pieceKey)
        {
            case Piece.PiecesEnum.Peo: return new Peo(board, team, pos, Piece.PiecesEnum.Peo, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Torre: return new Torre(board, team, pos, Piece.PiecesEnum.Torre, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Caball: return new Caball(board, team, pos, Piece.PiecesEnum.Caball, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Alfil: return new Alfil(board, team, pos, Piece.PiecesEnum.Alfil, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Reina: return new Reina(board, team, pos, Piece.PiecesEnum.Reina, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Rei: return new Rei(board, team, pos, Piece.PiecesEnum.Rei, board.AllTeams[team].isDefeated, hasmoved);
        }
        Debug.LogError("type of piece not in enum: " + pieceKey);
        return null;
    }
    public static bool isVector2inBoard(Vector2Int vector, Vector2Int maxTile)
    {
        if (vector.x > maxTile.x || vector.x < 0) { return false; }
        if (vector.y > maxTile.y || vector.y < 0) { return false; }
        return true;
    }
}
