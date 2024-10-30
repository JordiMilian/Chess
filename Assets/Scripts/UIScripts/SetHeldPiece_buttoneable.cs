using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeldPiece_buttoneable : MonoBehaviour, ILeftButtonaeble
{
    [SerializeField] Piece.PiecesEnum pieceType;
    [SerializeField] Editor_Controller controller;
    [SerializeField] PiecesInstantiator instantiator;
    [SerializeField] SpriteRenderer buttonSprite;
    [SerializeField] Animator buttonAnimator;
    [SerializeField] AudioClip selectedAudio;
    private void OnEnable()
    {
        controller.OnUpdatedHeldTeam += UpdateButtonColor;
        controller.OnUpdatedHeldPiece += UpdatedSelectedType;
    }
    private void OnDisable()
    {
        controller.OnUpdatedHeldTeam -= UpdateButtonColor;
        controller.OnUpdatedHeldPiece -= UpdatedSelectedType;
    }
    public void OnLeftClick()
    {
        controller.UpdateHeldType(pieceType);
        SFX_PlayerSingleton.Instance.playSFX(selectedAudio, 0.1f);
    }
    public void UpdateButtonColor(int teamIndex) 
    {
        Color color = controller.startingTeams[teamIndex].PiecesColor;
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
