using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_SaveSlot : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    EditorBoard thisEditor;
    Animator slotAnimator;

    private void Awake()
    {
        slotAnimator = GetComponent<Animator>();
    }
    public void OnLeftClick()
    {
        if(savingManager.isAttemptingSave) 
        {
            SaveBoard(editorController.MainEditorBoard);
            savingManager.OnSavedSlot(this);
        }
        else if(savingManager.isAttemptingLoad)
        {
            LoadThisBoard();
            savingManager.OnLoadedSlot(this); 
        }
    }
    public void ShowIsLastLoaded()
    {
        slotAnimator.SetBool("lastLoaded", true);
    }
    public void IsNotLastLoaded()
    {
        slotAnimator.SetBool("lastLoaded", false);
    }
    public void SaveBoard(EditorBoard editorBoard)
    {
        slotAnimator.SetTrigger("saved");

        thisEditor = new EditorBoard(
            editorBoard.maxTileWidth,
            editorBoard.maxTileHeight,
            editorBoard.maxActiveTiles,
            editorBoard.PiecesToSpawn,
            editorBoard.startingTeam,
            editorBoard.teamsDirs
            );
        //convertir a json o algo
    }
    public void LoadThisBoard()
    {
        slotAnimator.SetTrigger("saved");
        //load json o algo
        editorController.MainEditorBoard = new EditorBoard(
            thisEditor.maxTileWidth,
            thisEditor.maxTileHeight,
            thisEditor.maxActiveTiles,
            thisEditor.PiecesToSpawn,
            thisEditor.startingTeam,
            thisEditor.teamsDirs
            );
        editorController.LoadMainBoard();
    }
    void EditorBoardToJson(EditorBoard editorBoard)
    {

    }

}
