using UnityEngine;
using System; 
public class SoundManager : MonoBehaviour
{


    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource effectsSource;

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
        }

    }

    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume; 
        musicSource.Play();
    }

    public void PlayEffect(AudioClip clip) //float volume = 1.0f)
    {
        //oneshot is for sound effects that may overlap each other
        effectsSource.PlayOneShot(clip);   //, volume);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
