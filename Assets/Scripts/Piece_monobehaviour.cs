using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_monobehaviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer mainSprite, outlineSprite;
    [HideInInspector] public Piece pieceScript;
    [SerializeField] Animator pieceAnimator;
    [SerializeField] Color defeatedColor;
    [SerializeField] AudioClip movedClip;
    [SerializeField] AudioClip selectedClip;
    Color ownColor;
    Color ownOutline;
    float ownOutlineOpacity;
    bool permanentlyKillable;
    public void SetBaseColor(Color color)
    {
        ownColor = color;
    }
    void SetPieceColor(Color mainColor, Color outlineColor, float outlineOpacity)
    {
        mainSprite.color = mainColor;
        outlineSprite.color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, outlineOpacity);
    }
    public void OnSelectable()
    {
        float opacity = 1;
        SetPieceColor(ownColor, Color.white, opacity);
        ownOutline = Color.white;
        ownOutlineOpacity = opacity;
        pieceAnimator.SetTrigger("startOwnTurn");
    }
    public void OnGotSelected()
    {
        pieceAnimator.SetTrigger("startOwnTurn");
        Debug.Log(pieceScript.pieceEnum + " got selected");
        SFX_PlayerSingleton.Instance.playSFX(selectedClip);
    }
    public void OnUnselectable()
    {
        float opacity = 0f;
        SetPieceColor(ownColor, Color.white, opacity);
        ownOutline = Color.white;
        ownOutlineOpacity = opacity;
    }
    public void OnDefeated() 
    { 
        SetPieceColor(defeatedColor, Color.white, 0);
        ownColor = Color.gray;
    }
    public void OnKillable()
    {
        SetPieceColor(ownColor, Color.red, .5f);
        pieceAnimator.SetBool("isKillable", true);
    }
    public void OnNotKillable()
    {
        if (permanentlyKillable) { return; }
        if (pieceScript.isDefeated) { OnDefeated(); }
        else { SetPieceColor(ownColor, ownOutline, ownOutlineOpacity); }
        pieceAnimator.SetBool("isKillable", false);
    }
    public void OnPermanentlyKillable()
    {
        permanentlyKillable = true;
        OnKillable();
    }
    public void OnGotMoved()
    {
        pieceAnimator.SetTrigger("gotMoved");
        SFX_PlayerSingleton.Instance.playSFX(movedClip);
        Debug.Log("got moved");
    }
    public void OnAppeared()
    {
        OnUnselectable();
        pieceAnimator.SetTrigger("appeared");
    }
    public void OnHidden()
    {
        SetPieceColor(new Color(0, 0, 0, 0), Color.white, 0);
    }
}
