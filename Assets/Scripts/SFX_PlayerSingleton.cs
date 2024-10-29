
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFX_PlayerSingleton : MonoBehaviour
{
    List<AudioSource> audioSourcesList = new List<AudioSource>();

    public static SFX_PlayerSingleton Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        audioSourcesList = GetComponentsInChildren<AudioSource>().ToList<AudioSource>();
    }
    public void playSFX(AudioClip clip, float pitchVariationAdder = 0, float addedVolum = 0, float addedPitch = 0)//added volum should be a percent probably
    {
        AudioSource audioSource = GetFreeAudioSource();
        if(audioSource == null) { Debug.LogError("Not enough AudioSources, add more child sources to the singleton");return; }

        if (clip == null) { Debug.LogWarning("Missing audio clip: " + clip.name); return; }
        audioSource.pitch = 1;

        float randomAdder = Random.Range(-pitchVariationAdder, pitchVariationAdder);
        audioSource.pitch += randomAdder;
        audioSource.pitch += addedPitch;
        audioSource.volume += addedVolum;

        audioSource.clip = clip;
        audioSource.Play();

    }
    AudioSource GetFreeAudioSource()
    {
        AudioSource freeAudioSource = null;

        foreach (AudioSource source in audioSourcesList)
        {
            if (source.isPlaying) { continue; }

            freeAudioSource = source;
            break;
        }

        if(freeAudioSource == null) { Debug.LogWarning("No free audio source available, consider adding more source childs to the singleton"); }

        return freeAudioSource;
    }

}
