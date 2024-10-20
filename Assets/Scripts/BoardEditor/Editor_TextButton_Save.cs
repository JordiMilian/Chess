using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_TextButton_Save : Editor_BasicTextButton
{
    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    public override void OnPressedLogic()
    {
        savingManager.startAttemptingSave();
    }
    public override void OnReleaseLogic()
    {
        savingManager.cancelAttemptingSave();
    }
}
