using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Editor_BasicTextButton : MonoBehaviour,ILeftButtonaeble
{
    Color baseColor;
    SpriteRenderer boxSprite;
    public bool isHolding;
    public bool isInterruptor;
    Animator buttonAnimator;
    private void Awake()
    {
        boxSprite = GetComponent<SpriteRenderer>();
        baseColor = boxSprite.color;
        buttonAnimator = GetComponent<Animator>();
    }
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
    public void ForceUnrelease()
    {
        isHolding = false;
        OnReleasedFeedback();
        OnReleaseLogic();
    }
    private void OnMouseEnter()
    {
        buttonAnimator.SetBool("Hover", true);
    }
    private void OnMouseExit()
    {
        buttonAnimator.SetBool("Hover", false);
    }
   
    public void OnPressedFeedback()
    {
        buttonAnimator.SetBool("Pressed", true);
        buttonAnimator.SetBool("PressedTr", true);
    }
    public void OnReleasedFeedback()
    {
        buttonAnimator.SetBool("Pressed",false);
        boxSprite.color = baseColor;
    }
    public abstract void OnReleaseLogic();
    public abstract void OnPressedLogic();
    
}
