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
    [SerializeField] GameObject TileCross;
    private void Awake()
    {
        tileSprite = GetComponent<SpriteRenderer>();
        
        TileCross.SetActive(false);
    }
    public void OnHighlight()
    {
        SetTileColor(Color.white);
        //TileCircle.SetActive(true);
        TileCross.SetActive(false);
    }
    public void OnHighlightedButChecks()
    {
        SetTileColor(Color.white);
        TileCross.SetActive(true);
    }
    public void OnUnhightlight()
    {
        SetTileColor(ownColor);
        TileCross.SetActive(false);
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
