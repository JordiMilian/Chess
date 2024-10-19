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
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            leftButtoneable = gameObject.GetComponent<ILeftButtonaeble>();
            if (leftButtoneable != null)
            {
                leftButtoneable.OnLeftClick();
            }
        }
        if(Input.GetMouseButtonUp(1)) 
        {
            rightButtoneable = gameObject.GetComponent<IRightButtoneable>();
            if (rightButtoneable != null)
            {
                rightButtoneable.OnRightClick();
            }
        }
        


    }
}
