
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFX_PlayerSingleton : MonoBehaviour
{
    List<AudioSource> audioSourcesList = new List<AudioSource>();
    float BaseVolume;
    [SerializeField] AudioClip setBaseVolume_audio;

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
        BaseVolume = 0.5f;
        audioSourcesList = GetComponentsInChildren<AudioSource>().ToList<AudioSource>();
    }
    public void playSFX(AudioClip clip, float pitchVariationAdder = 0, float addedVolum = 0, float addedPitch = 0)//added volum should be a percent probably
    {
        AudioSource audioSource = GetFreeAudioSource();
        if(audioSource == null) { Debug.LogError("Not enough AudioSources, add more child sources to the singleton");return; }

        if (clip == null) { Debug.LogWarning("Missing audio clip: " + clip.name); return; }
        audioSource.pitch = 1;
        audioSource.volume = BaseVolume;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SetBaseVolume(0.1f); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SetBaseVolume(0.2f); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SetBaseVolume(0.3f); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SetBaseVolume(0.4f); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SetBaseVolume(0.5f); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SetBaseVolume(0.6f); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SetBaseVolume(0.7f); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SetBaseVolume(0.8f); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { SetBaseVolume(0.9f); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { SetBaseVolume(0f); }
    }
    void SetBaseVolume(float volume)
    {
        BaseVolume = volume;
        playSFX(setBaseVolume_audio, 0.1f);
    }
}
