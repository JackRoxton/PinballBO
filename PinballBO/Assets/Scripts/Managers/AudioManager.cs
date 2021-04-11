using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioClip music;

    private AudioSource musicSource;
    private AudioSource ambianceSource;
    private List<AudioSource> effectSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    
        ambianceSource = gameObject.GetComponent<AudioSource>();
        ambianceSource.volume = 10;
        ambianceSource.time = 78;
        ambianceSource.loop = true;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        if (music != null)
            PlayClip(musicSource, music);

        Bumper[] bumpers = FindObjectsOfType<Bumper>();
        foreach (Bumper bumper in bumpers)
        {
            AudioSource source = bumper.GetComponent<AudioSource>();
            effectSources.Add(source);

            source.playOnAwake = false;
            source.volume = 1;
            source.pitch = Random.Range(.7f, 1.2f);
        }
    }


    #region ChangeVolume
    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void ChangeEffectVolume(float volume)
    {
        foreach (AudioSource source in effectSources)
        {
            source.volume = volume;
        }
    }
    #endregion


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
