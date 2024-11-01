using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_TextButton_ReturnToEditing : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] GameController gameController;
    [SerializeField] Color mouseOverColor, defaulColor;
    SpriteRenderer sprite;
    [SerializeField] AudioClip mouseEnterAudio, clickedAudio;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        
    }
    private void OnEnable()
    {
        sprite.color = defaulColor;
    }
    public void OnLeftClick()
    {
        gameController.ReturnToEditing();
        SFX_PlayerSingleton.Instance.playSFX(clickedAudio);
    }
    private void OnMouseEnter()
    {
        sprite.color = mouseOverColor;
        SFX_PlayerSingleton.Instance.playSFX(mouseEnterAudio, 0.05f);
    }
    private void OnMouseExit()
    {
        sprite.color = defaulColor;
    }
}
