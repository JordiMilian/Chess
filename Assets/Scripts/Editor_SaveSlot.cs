using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Editor_SaveSlot : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] Editor_Controller editorController;
    [SerializeField] SavisngSystemManager savingManager;
    [SerializeField] AudioClip savingAudio, loadingAudio;
    Animator slotAnimator;

    private void Awake()
    {
        slotAnimator = GetComponent<Animator>();
        slotAnimator.keepAnimatorControllerStateOnDisable = true;
    }
    public void OnLeftClick()
    {
        if(savingManager.isAttemptingSave) 
        {
            SaveBoard(editorController.MainEditorBoard);
            savingManager.OnSavedSlot(this);
            SFX_PlayerSingleton.Instance.playSFX(savingAudio);
        }
        else if(savingManager.isAttemptingLoad)
        {
            LoadThisBoard();
            savingManager.OnLoadedSlot(this);
            SFX_PlayerSingleton.Instance.playSFX(loadingAudio);
        }
    }
    public void ShowIsLastLoaded()
    {
        slotAnimator.SetBool("lastLoaded", true);
    }
    public void HideSlot()
    {
        slotAnimator.SetBool("Hidden", true);
    }
    public void ShowSlot()
    {
        slotAnimator.SetBool("Hidden", false);
    }
    public void IsNotLastLoaded()
    {
        slotAnimator.SetBool("lastLoaded", false);
    }
    public void SaveBoard(EditorBoard editorBoard)
    {
        slotAnimator.SetTrigger("saved");


        int index = savingManager.getIndexOfSloat(this);
        string path = Application.persistentDataPath + "/jsonBoard" + index + ".json";
        
        File.WriteAllText(path, EditorToJsonString(editorBoard));

        /*
        thisEditor = new EditorBoard(
            editorBoard.maxTileWidth,
            editorBoard.maxTileHeight,
            editorBoard.maxActiveTiles,
            editorBoard.PiecesToSpawn,
            editorBoard.startingTeam,
            editorBoard.teamsDirs
            );
        */
    }
    public void LoadThisBoard()
    {
        slotAnimator.SetTrigger("saved");

        int index = savingManager.getIndexOfSloat(this);
        string path = Application.persistentDataPath + "/jsonBoard" + index + ".json";


        string jsonString = File.ReadAllText(path);
        editorController.MainEditorBoard = JsonStringToEditor(jsonString);

        editorController.LoadMainBoard();
    }
    string EditorToJsonString(EditorBoard editorBoard)
    {
        return JsonUtility.ToJson(editorBoard);
    }
    EditorBoard JsonStringToEditor(string jsonString)
    {
        return JsonUtility.FromJson<EditorBoard>(jsonString);
    }

}
