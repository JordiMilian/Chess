using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavisngSystemManager : MonoBehaviour
{
    public Action OnCanceledSaving;
    public bool isAttemptingLoad { get; private set; }
    public bool isAttemptingSave { get; private set; }
    int lastLoadedIndex = -1;
    
    [SerializeField] List<Editor_SaveSlot> saveSlotsList = new List<Editor_SaveSlot>();

    public void startAttempingLoad()
    {
        isAttemptingLoad = true;
        //saveSlotsList[lastLoadedIndex].ShowIsLastLoaded();
        cancelAttemptingSave();
        Debug.Log("start attempting load");
    }
    public void cancelAttemptingLoad()
    {
        isAttemptingLoad = false;
    }
    public void startAttemptingSave()
    {
        isAttemptingSave = true;
        cancelAttemptingLoad();
        Debug.Log("start attempting save");
    }
    public void cancelAttemptingSave()
    {
        isAttemptingSave = false;
        OnCanceledSaving?.Invoke();

    }
    public void OnSavedSlot(Editor_SaveSlot slot) //called from the slot itself
    {
        cancelAttemptingSave();
    }
    public void OnLoadedSlot(Editor_SaveSlot slot)
    {
        lastLoadedIndex = getIndexOfSloat(slot);
    }
    int getIndexOfSloat(Editor_SaveSlot slot)
    {
        for (int i = 0; i < saveSlotsList.Count; i++)
        {
            if(slot == saveSlotsList[i])
            {
                return i;
            }
        }
        Debug.LogError("slot not in list");
        return -1;
    }

}
