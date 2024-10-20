using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Editor_BasicTextButton : MonoBehaviour,ILeftButtonaeble
{
    
    bool isHolding;
    public bool isInterruptor;
    public void OnLeftClick()
    {
        if(isHolding)
        {
            OnReleasedFeedback();
            OnReleaseLogic();
            isHolding = false;
        }
        else
        {
            OnPressedFeedback();
            OnPressedLogic();
            if(isInterruptor)
            {
                isHolding = true;
            }
            else
            {
                OnReleasedFeedback();
                OnReleaseLogic();
            }
        }
    }
    public void OnPressedFeedback()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void OnReleasedFeedback()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    public abstract void OnReleaseLogic();
    public abstract void OnPressedLogic();
    
}
