using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoardDisplaySizeController_editor : MonoBehaviour
{
    [SerializeField] float DesiredMaxSize;
    [SerializeField] Transform editorRotTf;
    const float tileSize = 5.12f;

    public void SetSizeEditor(EditorBoard editor)
    {

        int largestSideTiles = Mathf.Max(editor.maxTileWidth, editor.maxTileHeight);

        float largestbaseLenght = largestSideTiles * tileSize;
        float desiredRootMultiplier = DesiredMaxSize / largestbaseLenght;
        

        editorRotTf.localScale = new Vector3(desiredRootMultiplier, desiredRootMultiplier, desiredRootMultiplier);
    }

    public void GetBasicSizeEditor()
    {
        editorRotTf.localScale = Vector3.one;
    }
}
