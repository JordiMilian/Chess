using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiecesInstantiator : MonoBehaviour
{
    public Board startingBoard;
    GameController gameController;
    
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
    public List<PieceCreator> PiecesToCreate = new List<PieceCreator>();
    public void CreatePieces()
    {
        gameController = GetComponent<GameController>();
        for (int i = 0; i < PiecesToCreate.Count; i++)
        {
            PieceCreator piece = PiecesToCreate[i];
            if (!gameController.gameBoard.AllTiles[piece.Position.x, piece.Position.y].isFree)
            {
                Debug.LogWarning("Tried to create piece " + piece.type + " on an not free place");
                continue;
            }
            if (piece.type == Piece.PiecesEnum.Rei)
            {
                if (gameController.gameBoard.AllTeams[piece.team].KingIndex != -1) { Debug.LogWarning("Tried to create two Kings for the same team"); continue; }
            }

            Piece newPiece = GetPieceByType(piece.type, gameController.gameBoard, piece.team, piece.Position, false);
            if (piece.type == Piece.PiecesEnum.Rei)
            {
                gameController.gameBoard.AllTeams[piece.team].KingIndex = i;
            }
        }
        gameController.gameBoard.UpdateKingsIndex();
    }
    public static Piece GetPieceByType(Piece.PiecesEnum pieceKey, Board board, int team, Vector2Int pos, bool hasmoved)
    {
        switch (pieceKey)
        {
            case Piece.PiecesEnum.Peo: return new Peo(board,team,pos, Piece.PiecesEnum.Peo, board.AllTeams[team].isDefeated,hasmoved); 
            case Piece.PiecesEnum.Torre: return new Torre(board,team,pos, Piece.PiecesEnum.Torre, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Caball: return new Caball(board,team,pos, Piece.PiecesEnum.Caball, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Alfil: return new Alfil(board,team,pos, Piece.PiecesEnum.Alfil, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Reina: return new Reina(board,team,pos, Piece.PiecesEnum.Reina, board.AllTeams[team].isDefeated, hasmoved);
            case Piece.PiecesEnum.Rei: return new Rei(board,team, pos,Piece.PiecesEnum.Rei, board.AllTeams[team].isDefeated, hasmoved);
        }
        Debug.LogError("type of piece not in enum: " + pieceKey);
        return null;
    }
}
