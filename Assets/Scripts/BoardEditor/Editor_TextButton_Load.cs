using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_TextButton_Load : Editor_BasicTextButton
{

    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    public override void OnPressedLogic()
    {
        savingManager.startAttempingLoad();
    }
    public override void OnReleaseLogic()
    {
        savingManager.cancelAttemptingLoad();
    }
}
