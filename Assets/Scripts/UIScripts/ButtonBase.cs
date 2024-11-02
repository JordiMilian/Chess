using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILeftButtonaeble
{
    public void OnLeftClick();
}
public interface IRightButtoneable 
{
    public void OnRightClick();
}
public class ButtonBase : MonoBehaviour
{
    ILeftButtonaeble leftButtoneable;
    IRightButtoneable rightButtoneable;
    float TimeToHold = 0.5f;
    float timeCounter;
    bool checkingHold;
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            leftButtoneable = gameObject.GetComponent<ILeftButtonaeble>();
            timeCounter = 0;
            checkingHold = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (leftButtoneable != null)
            {
                if(checkingHold)
                {
                    leftButtoneable.OnLeftClick();
                }
                
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(checkingHold)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter > TimeToHold)
                {
                    rightButtoneable = gameObject.GetComponent<IRightButtoneable>();
                    if (rightButtoneable != null)
                    {
                        rightButtoneable.OnRightClick();
                        checkingHold = false;
                        timeCounter = 0;
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(1)) 
        {
            rightButtoneable = gameObject.GetComponent<IRightButtoneable>();
            if (rightButtoneable != null)
            {
                rightButtoneable.OnRightClick();
            }
        }
    }
}
