using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeInDuration = 2.0f;
    public float displayDuration = 3.0f;
    public float fadeOutDuration = 2.0f;
    public string nextSceneName = "Level MainMenu Robb Scene 1";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (canvasGroup == null)
            return;

        StartCoroutine(PlayCompanyScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayCompanyScene()
    {
        //Fade in first
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        //Fade out last
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer/ fadeOutDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        SceneManager.LoadScene(nextSceneName);
    }

}
