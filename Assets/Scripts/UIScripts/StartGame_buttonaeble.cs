using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame_buttonaeble : Editor_BasicTextButton
{
    [SerializeField] GameController gameController;

    public override void OnPressedLogic()
    {
         StartCoroutine( gameController.StartPlaying());
    }
    public override void OnReleaseLogic()
    {
      
    }
}
