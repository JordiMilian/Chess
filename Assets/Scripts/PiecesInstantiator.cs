using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiecesInstantiator : MonoBehaviour
{
    GameController gameController;
    public enum PiecesEnum
    {
        Peo,Torre,Caball,Alfil,Reina,Rei
    }
    [Serializable]
    public class PieceCreation
    {
        public int team;
        public Vector2Int position;
        public PiecesEnum type;
    }
    [SerializeField] List<PieceCreation> PiecesToCreate = new List<PieceCreation>();
    public void CreatePieces()
    {
        gameController = GetComponent<GameController>();
        foreach (PieceCreation piece in PiecesToCreate)
        {
            
            if (!gameController.gameBoard.AllTiles[piece.position.x, piece.position.y].isFree)
            {
                Debug.LogWarning("Tried to create piece" + piece.type + " on an not free place");
                continue;
            }
            if(piece.type == PiecesEnum.Rei)
            {
                if (gameController.gameBoard.AllTeams[piece.team].King != null) { Debug.LogWarning("Tried to create two Kings for the same team"); continue; }
            }

            Piece newPiece = GetPieceByType(piece.type, gameController.gameBoard, piece.team, piece.position);
            if(piece.type == PiecesEnum.Rei)
            {
                gameController.gameBoard.AllTeams[piece.team].King = newPiece;
            }
        }
    }
    Piece GetPieceByType(PiecesEnum pieceKey, Board board, int team, Vector2Int pos)
    {
        switch (pieceKey)
        {
            case PiecesEnum.Peo: return new Peo(board,team,pos); 
            case PiecesEnum.Torre: return new Torre(board,team,pos);
            case PiecesEnum.Caball: return new Caball(board,team,pos);
            case PiecesEnum.Alfil: return new Alfil(board,team,pos);
            case PiecesEnum.Reina: return new Reina(board,team,pos);
            case PiecesEnum.Rei: return new Rei(board,team,pos);
        }
        Debug.LogError("type of piece not in enum: " + pieceKey);
        return null;
    }
   
    /*
    GameController gameState;
    [Serializable]
    public class PieceInstance
    {
        public Vector2Int coordinates;
        public GameObject PiecePrefab;
        public int Team;
    }
    [SerializeField] List<PieceInstance> piecesIntancesList = new List<PieceInstance>();

    private void Awake()
    {
         gameState = GameController.Instance;


        foreach (PieceInstance piece in piecesIntancesList )
        {
            if(piece.coordinates.x > gameState.boardWidth - 1 || piece.coordinates.y > gameState.boardHeight -1)
            {
                Debug.LogWarning("Coordinates out of range: " + piece.coordinates);
                continue;
            }

            Tile tileToSpawn = gameState.Board[piece.coordinates.x, piece.coordinates.y];
            Piece newPiece = CreateNewPiece(tileToSpawn, piece.PiecePrefab, piece.Team);

            if(newPiece is Rei)
            {
                if (gameState.AllTeams[piece.Team].King == null)
                {
                    gameState.AllTeams[piece.Team].King = newPiece;
                }
                else
                {
                    newPiece.onGettingEaten(); //Destroy king if there is already another
                }
            }
        }
    }
    public Piece CreateNewPiece(Tile tile, GameObject prefab, int team)
    {
        GameObject newPieceInst = Instantiate(prefab, tile.wordPosition, Quaternion.identity);
        Piece pieceScript = newPieceInst.GetComponent<Piece>();
        pieceScript.Team = team;
        pieceScript.UpdateOwnTile(tile);
        pieceScript.SetUpPiece();
        pieceScript.onPieceSelectedEvent += gameState.onSelected;
        pieceScript.onMovedPiece += gameState.onMoved;

        tile.UpdateTile(false, pieceScript);

        return pieceScript;
    }
    */
}
