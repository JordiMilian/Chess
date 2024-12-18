using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Editor_Controller : MonoBehaviour
{
    
    public EditorBoard MainEditorBoard;
    [SerializeField] GameObject EditorTilePrefab;
    [SerializeField] Editor_BoardDisplayer editorDisplayer;
    [SerializeField] float distanceBetweenTiles;
    [SerializeField] Transform startingPosTf;
    
    public int heldTeam;
    public Piece.PiecesEnum heldPiece;
    //public List<TeamClass> startingTeams = new List<TeamClass>();
    Editor_Tile_monobehaviour[,] editorTileMonos;
   
    public Action<int> OnUpdatedHeldTeam;
    public Action<Piece.PiecesEnum> OnUpdatedHeldPiece;
    public Action OnLoadedNewEditorBoard;
    //public List<PieceCreator> PiecesToCreate = new List<PieceCreator>();
    [Header("Audio")]
    [SerializeField] AudioClip CantPlaceOtherKingCLip;
    [SerializeField] AudioClip PlacedPieceClip, RemovedPieceClip, UpdatedBoardClip;
    [Serializable]
    public class PieceCreator
    {
        public int team;
        public Vector2Int Position;
        public Piece.PiecesEnum type;

        public PieceCreator(int cteam, Vector2Int cpos, Piece.PiecesEnum ctype)
        {
            team = cteam;
            Position = cpos;
            type = ctype;
        }
    }
    private void Awake()
    {
        heldTeam = 0;
        heldPiece = Piece.PiecesEnum.Peo;
    }
    public void LoadMainBoard()
    {
        MainEditorBoard.CreateTiles();
        editorDisplayer.CreateTilesPrefabs(MainEditorBoard);
        editorDisplayer.EnableAllDisaplays();
        OnUpdatedHeldPiece?.Invoke(heldPiece);
        OnUpdatedHeldTeam?.Invoke(heldTeam);
        OnLoadedNewEditorBoard?.Invoke();
    }
    public void StopEditing()
    {
        editorDisplayer.DisableAllDisplays();
    }
    public void OnTileLeftClicked(EditorBoard.EditorTile tile)
    {
        if(heldPiece == Piece.PiecesEnum.Rei)
        {
            for (int i = 0; i < MainEditorBoard.PiecesToSpawn.Count; i++)
            {
                if (MainEditorBoard.PiecesToSpawn[i].team == heldTeam && MainEditorBoard.PiecesToSpawn[i].type == Piece.PiecesEnum.Rei)
                {
                    Debug.LogWarning(MainEditorBoard.StartTeams[heldTeam].TeamName + " has a King alrady");
                    SFX_PlayerSingleton.Instance.playSFX(CantPlaceOtherKingCLip);
                    return;
                }
        }
        }
        MainEditorBoard.tryAddNewPiece(new PieceCreator(heldTeam, tile.Position, heldPiece));
        SFX_PlayerSingleton.Instance.playSFX(PlacedPieceClip,0.15f);
        editorDisplayer.UpdatePiecesDisplay(MainEditorBoard);
    }
    public void OnTileRightClicked(EditorBoard.EditorTile tile)
    {
        if(tile.isOcupied)
        {
            MainEditorBoard.DeletePieceAtPosition(tile.Position);
            editorDisplayer.UpdatePiecesDisplay(MainEditorBoard);
            SFX_PlayerSingleton.Instance.playSFX(RemovedPieceClip);
        }
        else
        {
            MainEditorBoard.UpdateActiveTiles(tile.Position);
            editorDisplayer.UpdateTilesDisplay(MainEditorBoard);
            editorDisplayer.UpdatePiecesDisplay(MainEditorBoard);
            SFX_PlayerSingleton.Instance.playSFX(UpdatedBoardClip);
        }
    }
    public Board EditorToBoard(EditorBoard editBoard)
    {

        return new Board(editBoard.maxActiveTiles.y +1,
            editBoard.maxActiveTiles.x+1,
            MainEditorBoard.StartTeams.ToList<TeamClass>(),//starting team is empty, we must create the pieces later
            editBoard.startingTeam,
            true
            ) ;
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
        MainEditorBoard.StartTeams[teamIndex].directionEnum = directionEnum;
    }
    
    public static void CreatePieces(List<PieceCreator> creators, Board board)
    {
        for (int i = 0; i < creators.Count; i++)
        {
            if (!StaticMethods.isVector2inBoard(creators[i].Position, new Vector2Int(board.Width-1, board.Height-1)))
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

            StaticMethods.CreatePieceByType(piece.type, board, piece.team, piece.Position, false);
            if (piece.type == Piece.PiecesEnum.Rei)
            {
                board.AllTeams[piece.team].KingIndex = i;
            }
        }
        board.UpdateKingsIndex();
    }
    public void ClearCurrentBoard()
    {
        MainEditorBoard.RestartBoard();
        editorDisplayer.CreateTilesPrefabs(MainEditorBoard);
        editorDisplayer.UpdatePiecesDisplay(MainEditorBoard);
        editorDisplayer.UpdateTilesDisplay(MainEditorBoard);
    }
}
