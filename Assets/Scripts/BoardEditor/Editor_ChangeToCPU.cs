using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_ChangeToCPU : MonoBehaviour, ILeftButtonaeble
{
    public int TeamIndex;
    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    [SerializeField] Transform Ui_CPU, Ui_Player;

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
        if(editorController.startingTeams[TeamIndex].isComputer)
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
        editorController.startingTeams[TeamIndex].isComputer = !editorController.startingTeams[TeamIndex].isComputer;
        SetUi();
    }
}
