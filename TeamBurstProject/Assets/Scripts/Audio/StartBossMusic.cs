using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class StartBossMusic : MonoBehaviour
    
{
    [SerializeField] public AudioMixer musicMixer;
    [SerializeField] public AudioClip bossMusic;
    [SerializeField] GameObject musicTrigger;
    [SerializeField] GameObject boss; 
    public string volumParamName = "MusicVolume";
    public float fadeDuration = 2.0f;
    private float currentVolume;
    private bool isFadingOut = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

     

        if (other.CompareTag("Player"))
        {
            FadeOutMusic();

        }

    }

    public void FadeOutMusic()
    {
        if (!isFadingOut)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFadingOut = true;
        musicMixer.GetFloat(volumParamName, out currentVolume);

        float timer = 0f;
        float updateInterval = 1f; 

        while (timer < fadeDuration)
        {
            
            float newVolume = Mathf.Lerp(currentVolume, -80f, timer / fadeDuration);
            musicMixer.SetFloat(volumParamName, newVolume);

            //timer += Time.deltaTime;
            timer += updateInterval;
            yield return null;
        }

        musicMixer.SetFloat(volumParamName, -80f); 
        isFadingOut = false;
        musicMixer.SetFloat(volumParamName, currentVolume);
        SoundManager.Instance.PlayMusic(bossMusic);
        musicTrigger.SetActive(false);

        if (boss != null)
            boss.SetActive(true);

    }



}
