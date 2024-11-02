using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButton_Help : Editor_BasicTextButton
{
    [SerializeField] Transform HelpImage;
    [SerializeField] Transform HelpImage_Phone;
    public override void OnPressedLogic()
    {
        if(Application.isMobilePlatform)
        {
            HelpImage_Phone.gameObject.SetActive(true);
        }
        else
        {
            HelpImage.gameObject.SetActive(true);
        }
        
    }
    public override void OnReleaseLogic()
    {
        if (Application.isMobilePlatform)
        {
            HelpImage_Phone.gameObject.SetActive(false);
        }
        else
        { 
            HelpImage.gameObject.SetActive(false);
        }
       
    }

}
