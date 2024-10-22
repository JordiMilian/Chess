using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_TextButton_Clear : Editor_BasicTextButton
{
    [SerializeField] Editor_Controller editorController;
    public override void OnPressedLogic()
    {
        editorController.ClearCurrentBoard();
    }
    public override void OnReleaseLogic()
    {
        
    }
}
