using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_SaveSlot : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    EditorBoard thisEditor;


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
    public void ShowIsLastLoaded()//Hauria de haver un manajer de saves maybe que mire quin ha sigut l'ultim save index LastSavedSlotManager maybe
    {
        //feedback del boto parpallejan
    }
    public void SaveBoard(EditorBoard editorBoard)
    {
        thisEditor = new EditorBoard(
            editorBoard.maxTileWidth,
            editorBoard.maxTileHeight,
            editorBoard.maxActiveTiles,
            editorBoard.PiecesToSpawn,
            editorBoard.startingTeam,
            editorBoard.teamsDirs
            );
        Debug.Log("Saved Board");
        //convertir a json o algo
    }
    public void LoadThisBoard()
    {
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
        Debug.Log("Loaded Board");
    }
    void EditorBoardToJson(EditorBoard editorBoard)
    {

    }

}
