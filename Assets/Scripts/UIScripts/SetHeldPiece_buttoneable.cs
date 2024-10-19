using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeldPiece_buttoneable : MonoBehaviour, IButtonaeble
{
    [SerializeField] Piece.PiecesEnum pieceType;
    [SerializeField] Editor_Controller controller;
    [SerializeField] PiecesInstantiator instantiator;
    [SerializeField] SpriteRenderer buttonSprite;
    private void OnEnable()
    {
        controller.OnUpdatedHeldTeam += UpdateButtonColor;
    }
    private void OnDisable()
    {
        controller.OnUpdatedHeldTeam -= UpdateButtonColor;
    }
    public void OnPressed()
    {
        controller.UpdateHeldType(pieceType);
    }
    public void UpdateButtonColor(int teamIndex) 
    {
        Color color = controller.startingTeams[teamIndex].PiecesColor;
        buttonSprite.color = color;
    }
}
