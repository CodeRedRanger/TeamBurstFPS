using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;




public class ButtonFunctions : MonoBehaviour
{
    //below is not needed. You can just call current scene from game manager
    Scene currentScene;
    [SerializeField] AudioClip nonStartButtonSound;
    [SerializeField] AudioClip StartButtonSound;


    public void resume()
    {
        SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
        gameManager.instance.stateUnpause();
    }

    public void restartLevel()
    {
        SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
        //reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.firstUnpause = true; 
        gameManager.instance.stateUnpause(); //in case paused when restarting level
    }

    public void quit()
    {
    
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
           //For Windows build
           Application.Quit(); //note: won't work in editor, only in build  
           //for web build
           //SceneManager.LoadScene(0);
#endif
    }

    public void respawn()
    {
        SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();    
    }

    public void loadLevel(int lvl)
    {

        StartCoroutine(WaitForSoundEffect(lvl));
        /*
        if (lvl == 1)
        {
            SoundManager.Instance.PlayEffect(StartButtonSound, 1f);
            StartCoroutine(WaitForSoundEffect(lvl));
            SceneManager.LoadScene(lvl);
           
        }
        else
        {
            SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
            SceneManager.LoadScene(lvl);
            
        }*/

        gameManager.instance.firstUnpause = true;
        gameManager.instance.stateUnpause(); //in case paused when changing level


    }

 
    IEnumerator WaitForSoundEffect(int lvl)
    {
        if (lvl == 1)
        {
            SoundManager.Instance.PlayEffect(StartButtonSound, 1f);
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(lvl);

        }
        else
        {
            SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(lvl);

        }

    }


}
