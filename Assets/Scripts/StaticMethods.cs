using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethods : MonoBehaviour
{
    public static Piece CreatePieceByType(Piece.PiecesEnum pieceKey, Board board, int team, Vector2Int pos, bool hasmoved)
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
