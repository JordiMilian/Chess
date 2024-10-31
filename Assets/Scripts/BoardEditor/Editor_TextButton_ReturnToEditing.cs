using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_TextButton_ReturnToEditing : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] GameController gameController;
    [SerializeField] Color mouseOverColor, defaulColor;
    SpriteRenderer sprite;
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
    }
    private void OnMouseEnter()
    {
        sprite.color = mouseOverColor;
    }
    private void OnMouseExit()
    {
        sprite.color = defaulColor;
    }
}
