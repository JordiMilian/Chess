using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_TextButton_ReturnToEditing : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] GameController gameController;
    public void OnLeftClick()
    {
        gameController.ReturnToEditing();

    }
}
