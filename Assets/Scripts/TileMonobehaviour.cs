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
    Animator tileAnimator;
    [SerializeField] AudioClip mouseOverSelectable;
    private void Awake()
    {
        tileSprite = GetComponent<SpriteRenderer>();
        tileAnimator = GetComponent<Animator>();
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
    public void OnAppear()
    {
        tileAnimator.SetTrigger("AppearTrigger");
        tileAnimator.SetBool("Appearing",true);
    }
    public void OnHidden()
    {
        SetTileColor(new Color(0,0,0,0));
    }
    private void OnMouseDown()
    {
        onTileClicked?.Invoke(tileScript);
        tileScript.TileGotClicked(tileScript);
    }
    private void OnMouseEnter()
    {
        if(!tileScript.isFree && tileScript.currentPiece.isSelectable)
        {
            SFX_PlayerSingleton.Instance.playSFX(mouseOverSelectable, 0.1f, -0.4f, -0.1f);
        }
        tileAnimator.SetTrigger("mouseOver");
    }

}
