using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class Tile 
{
    public Vector2Int Coordinates;
    public bool isFree;
    public Piece currentPiece;
    public Action<Tile> onClickedTile;
    public bool isPosibleTile;
    public bool isLegalTile;

   
    public Tile(Vector2Int coordinates)
    {
        Coordinates = coordinates;
        isFree = true;
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
    public void TileGotClicked(Tile tile)
    {
        onClickedTile?.Invoke(this);
    }
}
