using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Play_TextController : MonoBehaviour
{
    [SerializeField] TextMeshPro MainTextDisplayer;
    [SerializeField] TextMeshPro[] PlayersTextDisplayerArray;

    private void Awake()
    {
        MainTextDisplayer.text = "SETTING UP BOARD";
    }
    void UpdateTeamText(int index, string newText)
    {
        PlayersTextDisplayerArray[index].text = newText;
    }
    void UpdateMainText(string newText)
    {
        MainTextDisplayer.text = newText;
    }
    public void OnPlayersTurn(int index)
    {
        UpdateMainText(indexToPlayersName(index, true) + "'S TURN");
    }
    public void OnPlayerCheckMated(int index)
    {
        UpdateTeamText(index, indexToPlayersName(index) + " got DEFEATED - CheckMate");
    }
    public void OnPlayernoMorePieces(int index)
    {
        UpdateTeamText(index, indexToPlayersName(index) + " got DEFEATED - No more pieces");
    }
    public void OnPlayerNoMoreMoves(int index)
    {
        UpdateTeamText(index, indexToPlayersName(index) + " got DEFEATED - No moves available");
    }
    public void OnPlayerNotPlaying(int index)
    {
        UpdateTeamText(index, "   ");
    }
    public void OnPlayerInCheck(int index)
    {
        UpdateTeamText(index, indexToPlayersName(index) + " is in CHECK");
    }
    public void OnBoardWithNoTeams()
    {
        UpdateMainText("EMPTY BOARD");
        UpdateTeamText(0, "Nothing");
        UpdateTeamText(1, "to ");
        UpdateTeamText(2, "do");
        UpdateTeamText(3, "here");
        
    }
    public void OnBoardWithOneTeam()
    {
        UpdateMainText("BOARD WITH ONE PLAYER");
        UpdateTeamText(0, "What");
        UpdateTeamText(1, "did");
        UpdateTeamText(2, "you");
        UpdateTeamText(3, "expect?");
    }
    public void OnPlayerFine(int index)
    {
        UpdateTeamText(index, indexToPlayersName(index) + " is fine");
    }
    string indexToPlayersName(int index, bool mayus = false)
    {
        if (mayus)
        {
            switch (index)
            {
                case 0: return "RED";
                case 1: return "BLUE";
                case 2: return "GREEN";
                case 3: return "PINK";
            }
        }
        switch (index)
        {
            case 0: return "Red";
            case 1: return "Blue";
            case 2: return "Green";
            case 3: return "Pink";
        }
        return "---";
    }
    public void OnGameOver(GameController.GameState state)
    {
        UpdateMainText(indexToPlayersName(state.WinnerIndex, true) + " WON THE GAME");
        UpdateTeamText(state.WinnerIndex, indexToPlayersName(state.WinnerIndex) + " won");
    }
}
