using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    [SerializeField] private Toggle muteToggle;
    private bool disableToggleEvent;
    [SerializeField] AudioClip musicExample;
    [SerializeField] AudioClip SFXExample;
    private bool firstTime = true; 
    

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        muteToggle.onValueChanged.AddListener(HandleToggelValueChanged);
    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleToggelValueChanged(bool enableSound)
    {
        if (disableToggleEvent) return;

        if (enableSound)
            slider.value = slider.maxValue;
        else
            slider.value = slider.minValue;
    }

    private void OnDisable()
    {
        
        PlayerPrefs.SetFloat(volumeParameter, slider.value);

    }

    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);

        if (volumeParameter == "MasterVolume" || volumeParameter == "MusicVolume")
        {
            if (musicExample != null && firstTime == false)
                SoundManager.Instance.PlayMusic(musicExample, slider.value);

            firstTime = false;
        }
        else if (volumeParameter == "SFXVolume")
        {
            if (musicExample != null)
                SoundManager.Instance.StopMusic();

            if (SFXExample != null && firstTime == false)
                SoundManager.Instance.PlayEffect(SFXExample, slider.value);

            firstTime = false;
        }

        disableToggleEvent = true; 
        muteToggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;
    }

    
}
