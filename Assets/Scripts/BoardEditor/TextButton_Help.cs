using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButton_Help : Editor_BasicTextButton
{
    [SerializeField] Transform HelpImage;
    public override void OnPressedLogic()
    {
        HelpImage.gameObject.SetActive(true);
    }
    public override void OnReleaseLogic()
    {
        HelpImage.gameObject.SetActive(false);
    }

}
