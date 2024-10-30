using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCutscenes : MonoBehaviour
{
    [SerializeField] TextMeshPro mainText, subtitleText;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip defeatedClip;
    [SerializeField] AudioClip defeatedAudio, gameOverAudio, emptyBoardAudio;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator GameOverCutscene(int winnerIndex)
    {
        SetMainText("GAME OVER");
        SetSubtitle("Winner: "+ indexToPlayersName(winnerIndex, true));
        SFX_PlayerSingleton.Instance.playSFX(gameOverAudio);
        animator.SetTrigger("Defeated");
        float animationTime = defeatedClip.length;
        Debug.Log("Waiting for " + animationTime);
        yield return new WaitForSeconds(animationTime);
    }
    public IEnumerator PlayerDefeatedCoroutine(int defeatedIndex, string subtitle)
    {
        SetMainText(indexToPlayersName(defeatedIndex, true) + "  GOT DEFEATED");
        SetSubtitle(subtitle);
        SFX_PlayerSingleton.Instance.playSFX(defeatedAudio);
        animator.SetTrigger("Defeated");
        float animationTime = defeatedClip.length;
        Debug.Log("Waiting for " + animationTime);
        yield return new WaitForSeconds(animationTime);

    }
    public IEnumerator EmptyBoardCoroutine()
    {
        SetMainText("EMPTY BOARD");
        SetSubtitle("Are you stupid?");
        SFX_PlayerSingleton.Instance.playSFX(emptyBoardAudio);
        animator.SetTrigger("Defeated");
        float animationTime = defeatedClip.length;
        Debug.Log("Waiting for " + animationTime);
        yield return new WaitForSeconds(animationTime);
    }
    public IEnumerator OnlyOnePlayer()
    {
        SetMainText("ONLY ONE PLAYER");
        SetSubtitle("Enjoy yourself?");
        animator.SetTrigger("Defeated");
        float animationTime = defeatedClip.length;
        Debug.Log("Waiting for " + animationTime);
        yield return new WaitForSeconds(animationTime);
    }
    public IEnumerator LostWithOnePlayer()
    {
        SetMainText("YOU LOST ALONE?");
        SetSubtitle("Idiot");
        SFX_PlayerSingleton.Instance.playSFX(defeatedAudio);
        animator.SetTrigger("Defeated");
        float animationTime = defeatedClip.length;
        Debug.Log("Waiting for " + animationTime);
        yield return new WaitForSeconds(animationTime);
    }
    void SetMainText(string s)
    {
        mainText.text = s;
    }
    void SetSubtitle(string s)
    {
        subtitleText.text = s;
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
                case 3: return "PURPLE";
            }
        }
        switch (index)
        {
            case 0: return "Red";
            case 1: return "Blue";
            case 2: return "Green";
            case 3: return "Purple";
        }
        return "---";
    }
}
