using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMonobehaviour : MonoBehaviour
{
    public Tile tileScript;
    SpriteRenderer tileSprite;
    public Action<Tile> onTileClicked;
    private void Awake()
    {
        tileSprite = GetComponent<SpriteRenderer>();
    }
    public void OnHighlight()
    {
        tileSprite.color = Color.yellow;
    }
    public void OnUnhightlight()
    {
        tileSprite.color = Color.grey;
    }
    private void OnMouseDown()
    {
        onTileClicked?.Invoke(tileScript);
        tileScript.TileGotClicked(tileScript);
    }
}
