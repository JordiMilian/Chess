using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EditorBoard;

public class Editor_Tile_monobehaviour : MonoBehaviour
{
    public EditorTile thisEditorTile;
    [SerializeField] float TimeToHold;
    public Action<EditorTile> OnGotRightClicked;
    public Action<EditorTile> OnGotLeftClicked;
    [SerializeField] SpriteRenderer tileSprite;
    [SerializeField] Color activeColor1, activeColor2, unactiveColor1, unactiveColor2;

    public void OnTileActivated()
    {
        //thisEditorTile.isActive = true;
        if((thisEditorTile.Position.x + thisEditorTile.Position.y) % 2 != 0)
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
        if ((thisEditorTile.Position.x + thisEditorTile.Position.y) % 2 != 0)
        {
            tileSprite.color = unactiveColor1;
        }
        else
        {
            tileSprite.color = unactiveColor2;
        }
    }
    float timeCounter;
    bool checkingHold;
    private void OnMouseOver()
    {
        if(Application.isMobilePlatform) //Running in Phone
        {
            if (Input.GetMouseButtonDown(0))
            {
                timeCounter = 0;
                checkingHold = true;
            }
            if(Input.GetMouseButtonUp(0))
            {
                if(checkingHold)
                {
                    OnGotLeftClicked(thisEditorTile);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (checkingHold == true)
                {
                    timeCounter += Time.deltaTime;

                    if (timeCounter > TimeToHold)
                    {
                        timeCounter = 0;
                        checkingHold = false;
                        OnGotRightClicked(thisEditorTile);
                    }
                }
            }
        }
        else //PC
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnGotRightClicked(thisEditorTile);
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnGotLeftClicked(thisEditorTile);
            }
        }
        
    }
}
