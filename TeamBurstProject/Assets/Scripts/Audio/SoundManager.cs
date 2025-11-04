using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource effectsSource;

    //NEW
    public AudioMixer masterMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //so only one instance of SoundManager exists at any time
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //NEW
            float masterValue = PlayerPrefs.GetFloat("MasterVolume", 1);
            float masterVolume = Mathf.Log10(masterValue) * 30f; 
            masterMixer.SetFloat("MasterVolume", masterVolume);

        }

       


    }

    public void PlayMusic(AudioClip clip, float volume = 0.6f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayEffect(AudioClip clip, float volume) //= 1.0f)
    {
        //oneshot is for sound effects that may overlap each other
        effectsSource.PlayOneShot(clip, volume);
    }

    public void PlayEffectDelayed(AudioClip clip, float volume, float delay)
    {
        effectsSource.PlayDelayed(delay);
        effectsSource.PlayOneShot(clip, volume);
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void ChangeVolumeMusic(float newVolume)
    {
        musicSource.volume = Mathf.Clamp(newVolume, 0f, 1f);

    }

    public bool MusicIsPlaying()
    {
        return musicSource.isPlaying;
    }

    //NEW
    public void SetMasterVolume(float value)
    {
        float volume = Mathf.Log10(value) * 30f; 
        masterMixer.SetFloat("MasterVolume", volume);
        //masterMixer.SetFloat("MasterVolume", volume);
        //mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); 
    }

    //END NEW
}



