using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButton_Quit : Editor_BasicTextButton
{
    public override void OnPressedLogic()
    {
        Application.Quit();
    }
    public override void OnReleaseLogic()
    {
        
    }
}
