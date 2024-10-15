using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public Vector2Int Coordinates;
    public bool isFree;
    public Piece currentPiece;
    public Action<Tile> onClickedTile;
    public bool isHighlighted;

    public Tile(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }
    public void UpdateTile(bool isfree, Piece newPiece = null)
    {
        if(isfree) 
        {
            isFree = true;
            currentPiece = null;
        }
        else
        {
            isFree = false;
            currentPiece = newPiece;
        }
    }
}
