using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioClip music;

    public float musicVolume = 1;
    public float effectVolume = 1;
    private AudioSource musicSource;
    private AudioSource ambianceSource;
    private List<AudioSource> effectSources = new List<AudioSource>();

    public Slider musicSlider;
    public Slider effectSlider;

    void Start()
    {
        effectVolume = GameManager.Instance.effectVolume;
        musicVolume = GameManager.Instance.musicVolume;

        if (Instance != null) Destroy(gameObject);
        Instance = this;
    
        ambianceSource = gameObject.GetComponent<AudioSource>();
        ambianceSource.volume = 10;
        ambianceSource.time = 78;
        ambianceSource.loop = true;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        if (music != null)
            PlayClip(musicSource, music);

        Bumper[] bumpers = FindObjectsOfType<Bumper>();
        foreach (Bumper bumper in bumpers)
        {
            AudioSource source = bumper.GetComponent<AudioSource>();
            effectSources.Add(source);

            source.playOnAwake = false;
            source.volume = effectVolume;
            source.pitch = Random.Range(.7f, 1.2f);
        }

        //update the sliders for each scene
        musicSlider.value = musicVolume;
        effectSlider.value = effectVolume;

    }


    #region ChangeVolume
    public void ChangeMusicVolume(float volume)
    {
        GameManager.Instance.StoreMusicVolume(volume);
        musicSource.volume = volume;
    }
    public void ChangeEffectVolume(float volume)
    {
        GameManager.Instance.StoreEffectVolume(volume);
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
