using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavisngSystemManager : MonoBehaviour
{
    public Action OnCanceledSaving;
    public bool isAttemptingLoad { get; private set; }
    public bool isAttemptingSave { get; private set; }
    [SerializeField] Editor_TextButton_Save saveButton;
    [SerializeField] Editor_TextButton_Load loadButton;
    int lastLoadedIndex = -1;
    
    [SerializeField] List<Editor_SaveSlot> saveSlotsList = new List<Editor_SaveSlot>();

    public void startAttempingLoad()
    {
        isAttemptingLoad = true;
        if (saveButton.isHolding) { saveButton.ForceUnrelease(); }
        Debug.Log("start attempting load");
        showAllSlots();
    }
    public void cancelAttemptingLoad()
    {
        isAttemptingLoad = false;
        if (loadButton.isHolding) { loadButton.ForceUnrelease(); }
        hideAllSlots();
    }
    public void startAttemptingSave()
    {
        isAttemptingSave = true;
        if (loadButton.isHolding) { loadButton.ForceUnrelease(); }
        Debug.Log("start attempting save");
        if (lastLoadedIndex > -1) { saveSlotsList[lastLoadedIndex].ShowIsLastLoaded(); }
        showAllSlots();
    }
    public void cancelAttemptingSave()
    {
        isAttemptingSave = false;
        if (saveButton.isHolding) { saveButton.ForceUnrelease(); }
        if (lastLoadedIndex > -1) { saveSlotsList[lastLoadedIndex].IsNotLastLoaded(); }
        hideAllSlots();
    }
    public void OnSavedSlot(Editor_SaveSlot slot) //called from the slot itself
    {
        cancelAttemptingSave();
    }
    public void OnLoadedSlot(Editor_SaveSlot slot)
    {
        cancelAttemptingLoad();
    
        lastLoadedIndex = getIndexOfSloat(slot);
        Debug.Log("loaded board in index: " + lastLoadedIndex);
    }
    public int getIndexOfSloat(Editor_SaveSlot slot)
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
    void showAllSlots()
    {
        foreach(Editor_SaveSlot slot in  saveSlotsList)
        {
            slot.ShowSlot();
        }
    }
    void hideAllSlots()
    {
        foreach (Editor_SaveSlot slot in saveSlotsList)
        {
            slot.HideSlot();
        }
    }

}
