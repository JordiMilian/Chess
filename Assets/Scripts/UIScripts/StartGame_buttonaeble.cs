using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame_buttonaeble : MonoBehaviour,ILeftButtonaeble
{
    [SerializeField] GameController gameController;

    public void OnLeftClick()
    {
        gameController.StartPlaying();
    }
}
