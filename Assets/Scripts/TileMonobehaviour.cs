using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMonobehaviour : MonoBehaviour
{
    public Tile tileScript;
    SpriteRenderer tileSprite;
    public Action<Tile> onTileClicked;
    public Color ownColor;
    private void Awake()
    {
        tileSprite = GetComponent<SpriteRenderer>();
    }
    public void OnHighlight()
    {
        SetTileColor(Color.white);
    }
    public void OnUnhightlight()
    {
        SetTileColor(ownColor);
    }
    void SetTileColor(Color color)
    {
        tileSprite.color=color;
    }
    public void SetBaseColor(Color color)
    {
        SetTileColor(color);
        ownColor = color;
    }
    private void OnMouseDown()
    {
        onTileClicked?.Invoke(tileScript);
        tileScript.TileGotClicked(tileScript);
    }

}
