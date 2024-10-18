using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeldTeam_buttoneable : MonoBehaviour,IButtonaeble
{
    [SerializeField] int TeamIndex;
    [SerializeField] Editor_Controller controller;
    public void OnPressed()
    {
        controller.UpdateHeldTeam(TeamIndex);
    }
}
