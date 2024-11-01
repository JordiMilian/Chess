using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_ChangeToCPU : MonoBehaviour, ILeftButtonaeble
{
    public int TeamIndex;
    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    [SerializeField] Transform Ui_CPU, Ui_Player;
    [SerializeField] AudioClip clickedAudio;

    private void Awake()
    {
        savingManager.OnLoadedAction += SetUi;
    }
    private void Start()
    {
        SetUi();
    }
    void SetUi()
    {
        
        if(editorController.MainEditorBoard.StartTeams[TeamIndex].isComputer)
        {
            Ui_CPU.gameObject.SetActive(true);
            Ui_Player.gameObject.SetActive(false);
        }
        else
        {
            Ui_CPU.gameObject.SetActive(false);
            Ui_Player.gameObject.SetActive(true);
        }
    }
    public void OnLeftClick()
    {
        SFX_PlayerSingleton.Instance.playSFX(clickedAudio, 0.1f);
        editorController.MainEditorBoard.StartTeams[TeamIndex].isComputer = !editorController.MainEditorBoard.StartTeams[TeamIndex].isComputer;
        SetUi();
    }
}
