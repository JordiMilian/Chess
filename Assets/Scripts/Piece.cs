using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

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

    Tile[] currentValidTiles = new Tile[0];
    public abstract Tile[] GetMovableTiles(Vector2Int startingPos);
    //public abstract Tile[] GetDangerousTiles(); Dema ho fas. Aixo es NOMES pels peons
    public bool isVector2Valid(Vector2Int vector)
    {
        if (vector.x > ownBoard.Width - 1 || vector.x < 0) { return false; }
        if (vector.y > ownBoard.Height - 1 || vector.y < 0) { return false; }
        return true;
    }

    public Piece(Board board, int team, Vector2Int position)
    {
        ownBoard = board;
        Team = team;
        Position = position;
        ownBoard.AllTeams[Team].piecesList.Add(this);
        ownBoard.AllTiles[position.x, position.y].UpdateTile(false, this);
        UpdateOwnTile(ownBoard.AllTiles[position.x, position.y]);
    }
    public void MovePiece(Tile newTile)
    {
        currentTile.UpdateTile(true);
        if (!newTile.isFree)
        {
            newTile.currentPiece.onGettingEaten();
        }

        newTile.UpdateTile(false, this);
        UpdateOwnTile(newTile);

        onMovedPiece?.Invoke();

    }
    public void OnPieceSelected()
    {
        currentValidTiles = GetMovableTiles(currentTile.Coordinates);

        foreach (Tile tile in currentValidTiles)
        {
            tile.isHighlighted = true;
            tile.onClickedTile += MovePiece;
        }
    }
    public void OnPieceUnselected()
    {
        foreach(Tile tile in currentValidTiles)
        {
            tile.isHighlighted = false;
            tile.onClickedTile -= MovePiece;
        }
        currentValidTiles = new Tile[0];
    }
    public void UpdateOwnTile(Tile newTile)
    {
        currentTile = newTile;
    }
    public void onGettingEaten()
    {
        ownBoard.AllTeams[Team].piecesList.Remove(this);
    }
}
