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

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        muteToggle.onValueChanged.AddListener(HandleToggelValueChanged); 
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
        disableToggleEvent = true; 
        muteToggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;
    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
