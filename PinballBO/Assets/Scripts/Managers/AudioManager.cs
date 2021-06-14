using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource effectSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambianceSource;

    private List<AudioSource> effectSources = new List<AudioSource>();

    [Space]
    public float musicVolume = 1;
    public float effectVolume = 1;
    [Space]
    public Slider musicSlider;
    public Slider effectSlider;


    [SerializeField] private AudioClip music;

    void Start()
    {
        effectVolume = GameManager.Instance.effectVolume;
        musicVolume = GameManager.Instance.musicVolume;

        if (Instance != null) Destroy(gameObject);
        Instance = this;

        ambianceSource.volume = 10;
        ambianceSource.time = 78;
        ambianceSource.loop = true;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        effectSource.volume = effectVolume;

        if (music != null)
            PlayMusic(music);

        #region Effect Sources Initialisation
        Bumper[] bumpers = FindObjectsOfType<Bumper>();
        foreach (Bumper bumper in bumpers)
        {
            AudioSource source = bumper.GetComponent<AudioSource>();
            effectSources.Add(source);

            source.playOnAwake = false;
            source.volume = effectVolume;
            source.pitch = Random.Range(.7f, 1.2f);
        }

        Targets[] targets = FindObjectsOfType<Targets>();
        foreach (Targets target in targets)
        {
            AudioSource source = target.GetComponent<AudioSource>();
            effectSources.Add(source);

            source.playOnAwake = false;
            source.volume = effectVolume;
            source.pitch = Random.Range(.7f, 1.2f);
        }
        #endregion

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
        effectSource.volume = volume;
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
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    #endregion
}
