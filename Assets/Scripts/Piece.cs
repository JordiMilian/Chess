using System;
using UnityEngine;
using static Board;

[Serializable]
public abstract class Piece
{
    [HideInInspector] public Board ownBoard;
    public Tile currentTile;
    public Vector2Int Position;
    public int Team;
    public bool isSelectable;
    public Action onMovedPiece;
    public Action onPieceSelectedEvent;
    public PiecesEnum pieceEnum;
    public enum PiecesEnum
    {
        Peo, Torre, Caball, Alfil, Reina, Rei
    }

    Movement[] currentValidMovements = new Movement[0];
    public abstract Movement[] GetAllPosibleMovements(Vector2Int startingPos);
    public abstract Tile[] GetDangerousTiles(); 
    public bool isVector2inBoard(Vector2Int vector)
    {
        if (vector.x > ownBoard.Width - 1 || vector.x < 0) { return false; }
        if (vector.y > ownBoard.Height - 1 || vector.y < 0) { return false; }
        return true;
    }

    public Piece(Board board, int team, Vector2Int position, PiecesEnum enume)
    {
        ownBoard = board;
        Team = team;
        Position = position;
        ownBoard.AllTeams[Team].piecesList.Add(this);
        ownBoard.AllTiles[position.x, position.y].UpdateTile(false, this);
        UpdateOwnTile(ownBoard.AllTiles[position.x, position.y]);
        pieceEnum = enume;
    }
    public void CallMovedEvent()
    {
        onMovedPiece?.Invoke();
    }
    public void OnPieceSelected()
    {
        currentValidMovements = GetAllPosibleMovements(currentTile.Coordinates);

        foreach (Movement mov in currentValidMovements)
        {
            ownBoard.AllTiles[mov.endPos.x, mov.endPos.y].isHighlighted = true;
            //tile.onClickedTile += MovePiece;
        }
    }
    public void OnPieceUnselected()
    {
        foreach(Movement mov in currentValidMovements)
        {
            ownBoard.AllTiles[mov.endPos.x, mov.endPos.y].isHighlighted = false;
            //tile.onClickedTile -= MovePiece;
        }
        currentValidMovements = new Movement[0];
    }
    public void UpdateOwnTile(Tile newTile)
    {
        currentTile = newTile;
        Position = newTile.Coordinates;
    }
    public void onGettingEaten()
    {
        ownBoard.AllTeams[Team].piecesList.Remove(this);
    }
}
