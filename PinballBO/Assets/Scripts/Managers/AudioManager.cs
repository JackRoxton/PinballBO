using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource ambianceSource;

    private List<AudioSource> effectSources = new List<AudioSource>();

    public float musicVolume = 1;
    public float effectVolume = 1;
    public Slider musicSlider;
    public Slider effectSlider;

    public string[] Sounds = new string[]
{
        "Coin",
        "Bump",
};

    [SerializeField]
    private List<AudioClip> audioArray;

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

        if (music != null)
            PlayMusic(musicSource, music);

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

    public AudioClip GetAudioCLip(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                return audioArray[i];
            }
        }
        return null;
    }

    public void StopClip(AudioSource source)
    {
        source.Stop();
    }

    public void PauseClip(AudioSource source)
    {
        source.Pause();
    }
    public void PlayMusic(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    public void Play(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                source.PlayOneShot(audioArray[i], effectVolume);
            }
        }
    }

    #endregion
}
