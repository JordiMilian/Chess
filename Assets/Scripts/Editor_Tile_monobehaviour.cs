using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EditorBoard;

public class Editor_Tile_monobehaviour : MonoBehaviour
{
    public EditorTile thisEditorTile;

    public Action<EditorTile> OnGotRightClicked;
    public Action<EditorTile> OnGotLeftClicked;
    [SerializeField] SpriteRenderer tileSprite;
    [SerializeField] Color activeColor1, activeColor2, unactiveColor1, unactiveColor2;

    public void OnTileActivated()
    {
        //thisEditorTile.isActive = true;
        if((thisEditorTile.Position.x + thisEditorTile.Position.y) % 2 == 0)
        {
            tileSprite.color =  activeColor1;
        }
        else
        {
            tileSprite.color = activeColor2;
        }
    }
    public void OnTileUnactivated()
    {
        //isActive = false;
        if ((thisEditorTile.Position.x + thisEditorTile.Position.y) % 2 == 0)
        {
            tileSprite.color = unactiveColor1;
        }
        else
        {
            tileSprite.color = unactiveColor2;
        }
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right clicked editor tile: " + thisEditorTile.Position);
            OnGotRightClicked(thisEditorTile);
        }
        if(Input.GetMouseButtonDown(0)) 
        {
            OnGotLeftClicked(thisEditorTile);
            Debug.Log("Left clicked editor tile: " + thisEditorTile.Position);
        }
    }
}
