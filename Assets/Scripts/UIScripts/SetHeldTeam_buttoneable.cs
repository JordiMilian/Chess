using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeldTeam_buttoneable : MonoBehaviour,ILeftButtonaeble,IRightButtoneable
{
    [SerializeField] int TeamIndex;
    [SerializeField] Editor_Controller controller;
    [SerializeField] Animator buttonAnimator;
    [SerializeField] TeamClass.directions currentDir;
    [SerializeField] AudioClip selectedAudio, turntAudio;
    private void Awake()
    {
        controller.OnLoadedNewEditorBoard += SetArrowAsEditor;
    }
    private void OnEnable()
    {
        controller.OnUpdatedHeldTeam += UpdateColorOutline;
        SetArrowAsEditor();
    }
    void SetArrowAsEditor()
    {
        currentDir = controller.MainEditorBoard.teamsDirs[TeamIndex];
        checkAnimator();
    }
    void UpdateColorOutline(int i)
    {
        if(i ==  TeamIndex)
        {
            buttonAnimator.SetBool("outline", true);
        }
        else
        {
            buttonAnimator.SetBool("outline", false);
        }
    }
    public void OnLeftClick()
    {
        controller.UpdateHeldTeam(TeamIndex);
        SFX_PlayerSingleton.Instance.playSFX(selectedAudio, 0.1f);
    }
    public void OnRightClick()
    {
        TeamClass.directions nextDir = getNextDirection(currentDir);
        controller.UpdateDiretion(TeamIndex, nextDir);
        currentDir = nextDir;
        checkAnimator();
        SFX_PlayerSingleton.Instance.playSFX(turntAudio, 0.1f);
    }
    void checkAnimator()
    {
        if(currentDir == TeamClass.directions.up) { buttonAnimator.SetTrigger("up"); }
        else if(currentDir == TeamClass.directions.down) { buttonAnimator.SetTrigger("down"); }
        else if(currentDir == TeamClass.directions.left) { buttonAnimator.SetTrigger("left"); } 
        else if(currentDir == TeamClass.directions.right) { buttonAnimator.SetTrigger("right");}
    }
    TeamClass.directions getNextDirection(TeamClass.directions direction)
    {
        switch (direction)
        {
            case TeamClass.directions.up: return TeamClass.directions.right;
            case TeamClass.directions.right: return TeamClass.directions.down;
            case TeamClass.directions.down: return TeamClass.directions.left;
            case TeamClass.directions.left: return TeamClass.directions.up;
        }
        return TeamClass.directions.up;
    }
}
