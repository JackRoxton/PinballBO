using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicSource;
    private List<AudioSource> effectSources;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    
        musicSource = gameObject.AddComponent<AudioSource>();
        Bumper[] bumpers = FindObjectsOfType<Bumper>();
        foreach (Bumper bumper in bumpers)
        {
            effectSources.Add(bumper.GetComponent<AudioSource>());
        }
    }


    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void ChangeEffectVolume(float volume)
    {
        foreach (AudioSource effect in effectSources)
        {
            effect.volume = volume;
        }
    }


    #region Clip
    public void PlayClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void StopClip(AudioSource source)
    {
        source.Stop();
    }

    public void PauseClip(AudioSource source)
    {
        source.Pause();
    }
    #endregion
}
