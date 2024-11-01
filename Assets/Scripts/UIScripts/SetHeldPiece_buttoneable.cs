using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeldPiece_buttoneable : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] Piece.PiecesEnum pieceType;
    [SerializeField] Editor_Controller editorController;
    [SerializeField] PiecesInstantiator instantiator;
    [SerializeField] SpriteRenderer buttonSprite;
    [SerializeField] Animator buttonAnimator;
    [SerializeField] AudioClip selectedAudio;
    private void OnEnable()
    {
        editorController.OnUpdatedHeldTeam += UpdateButtonColor;
        editorController.OnUpdatedHeldPiece += UpdatedSelectedType;
    }
    private void OnDisable()
    {
        editorController.OnUpdatedHeldTeam -= UpdateButtonColor;
        editorController.OnUpdatedHeldPiece -= UpdatedSelectedType;
    }
    public void OnLeftClick()
    {
        editorController.UpdateHeldType(pieceType);
        SFX_PlayerSingleton.Instance.playSFX(selectedAudio, 0.1f);
    }
    public void UpdateButtonColor(int teamIndex) 
    {
        Color color = editorController.MainEditorBoard.StartTeams[teamIndex].PiecesColor;
        buttonSprite.color = color;
    }
    public void UpdatedSelectedType(Piece.PiecesEnum piecesEnum)
    {
        if(pieceType == piecesEnum)
        {
            buttonAnimator.SetBool("outline", true);
        }
        else
        {
            buttonAnimator.SetBool("outline", false);
        }
    }
}
