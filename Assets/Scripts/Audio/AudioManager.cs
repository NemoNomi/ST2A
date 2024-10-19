using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource UISource;


    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip PauseMenuOpen;
    public AudioClip PauseMenuClose;
    public AudioClip UISelect;
    public AudioClip UISubmit;
    public AudioClip UIHover;
    public AudioClip Leaderboard;
    public AudioClip StorageDrop;
    public AudioClip Typewriter;

    #region Singleton
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {
        UISource.PlayOneShot(clip);
    }

    public void SetLowpassFilter(float frequency)
    {
        audioMixer.SetFloat("LowpassFreq", frequency);
    }
}
