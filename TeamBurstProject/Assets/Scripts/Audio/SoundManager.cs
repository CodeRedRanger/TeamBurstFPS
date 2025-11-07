using System;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource effectsSource;
    [HideInInspector] public float masterValue;
    [HideInInspector] public float musicValue;
    [HideInInspector] public float SFXValue;

    //NEW
    public AudioMixer masterMixer;
    private const float MinVolumeLinear = 0.0001f;
    private const float MaxVolumeLinear = 1f; 

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
            //GetComponent<AudioSource>().ignoreListenerPause = true; 

            LoadVolumes(); 

            //NEW
            /*
            float masterValue = PlayerPrefs.GetFloat("MasterVolume", 1);
            float masterVolume = Mathf.Log10(masterValue) * 30f; 
            if (masterVolume < -80)
                masterVolume = -80; 
            masterMixer.SetFloat("MasterVolume", masterVolume);
            //Debug.Log("MasterVolume set to " + masterVolume);
            if(masterMixer.GetFloat("MasterVolume", out float value))
                Debug.Log($"Current volume of '{"MasterVolume"}' is {value} dB");*/

        }

       


    }

    public void PlayMusic(AudioClip clip, float volume = 0.6f)
    {
        volume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayEffect(AudioClip clip, float volume) //= 1.0f)
    {
        volume = PlayerPrefs.GetFloat("SFXVolume", volume); 
        //oneshot is for sound effects that may overlap each other
        effectsSource.PlayOneShot(clip, volume);
    }

    public void PlayEffectDelayed(AudioClip clip, float volume, float delay)
    {
        volume = PlayerPrefs.GetFloat("SFXVolume", volume);
        effectsSource.PlayDelayed(delay);
        effectsSource.PlayOneShot(clip, volume);
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void ChangeVolumeMusic(float linearVolume)
    {
        float clampedVolume = Mathf.Clamp(linearVolume, MinVolumeLinear, MaxVolumeLinear);
        float dbVolume = Mathf.Log10(clampedVolume) * 30f;
        masterMixer.SetFloat("MusicVolume", dbVolume); 

    }

    public void LowerVolumeInstantly()
    {
        ChangeVolumeMusic(0.25f); 
    }

    public bool MusicIsPlaying()
    {
        bool isPlaying = false; 
        if(musicSource.isPlaying)
        {
            isPlaying = true;
        }

        return isPlaying; 
    }

    //NEW
    public void SetMasterVolume(float value)
    {
        float volume = Mathf.Log10(value) * 30f; 
        bool result = masterMixer.SetFloat("MasterVolume", volume);
        //masterMixer.SetFloat("MasterVolume", volume);
        //mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        if (!result)
        {
            Debug.LogError("Failed to set MasterVolume on AudioMixer");
        }
        else
            Debug.Log("MasterVolume set to " + volume);
        PlayerPrefs.Save();

    }

    public void LoadVolumes()
    {
        masterValue = PlayerPrefs.GetFloat("MasterVolume", 1);
        float masterVolume = Mathf.Log10(masterValue) * 30f;
        if (masterVolume < -80)
            masterVolume = -80;
        masterMixer.SetFloat("MasterVolume", masterVolume);
        //Debug.Log("MasterVolume set to " + masterVolume);
        //if (masterMixer.GetFloat("MasterVolume", out float value))
        //    Debug.Log($"Current volume of '{"MasterVolume"}' is {value} dB");

        musicValue = PlayerPrefs.GetFloat("MusicVolume", 1);
        float musicVolume = Mathf.Log10(musicValue) * 30f;
        if (musicVolume < -80)
            musicVolume = -80;
        masterMixer.SetFloat("MusicVolume", musicVolume);

        SFXValue = PlayerPrefs.GetFloat("SFXVolume", 1);
        float SFXVolume = Mathf.Log10(SFXValue) * 30f;
        if (SFXVolume < -80)
            SFXVolume = -80;
        masterMixer.SetFloat("SFXVolume", SFXVolume);

    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); 
    }

    //END NEW

    //Wrapper functions for button hover
    public void PlayButtonSelectedSound(AudioClip hoverSound)
    {
        if (effectsSource != null && effectsSource.enabled && effectsSource.gameObject.activeInHierarchy)
        {
            PlayEffect(hoverSound, 1.0f);
            //Debug.Log("Playing sound!");

        }
        else
        {
            //Debug.Log("effectsSource is null or disabled, cannot play sound.");
        }


    }
}



