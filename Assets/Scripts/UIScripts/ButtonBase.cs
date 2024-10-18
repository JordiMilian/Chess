using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonaeble
{
    public void OnPressed();
}
public class ButtonBase : MonoBehaviour
{
    IButtonaeble buttoneableScript;
    private void OnMouseDown()
    {
        buttoneableScript = gameObject.GetComponent<IButtonaeble>();
        buttoneableScript.OnPressed();
    }
}
