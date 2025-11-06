using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;





public class ButtonFunctions : MonoBehaviour
{
    //make enum later
    private int mainMenu = 0;
    private int playground = 1;
    //private int library = 2;
    //private int lunchroom = 5;
    //private int launchpad = 6;
    //private int alienship = 4;
    private int credits = 3;
    //private int options = 7;
    private int company = 8;



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
        //reset you items
        gameManager.instance.SaveItemStatus("Bomb", false);

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

    //continue button
    public void nextLevel()
    {
        int lvl = gameManager.instance.currentScene.buildIndex; 
        lvl += 1;
        loadLevel(lvl);
    }


    public void loadLevel(int lvl)
    {
        //save your items
        gameManager.instance.SaveItemStatus("Bomb", gameManager.instance.enableBomb);

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
        if (lvl == playground)
        {
            SoundManager.Instance.PlayEffect(StartButtonSound, 1f);
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(lvl);

        }
        else if (lvl == mainMenu)
        {

           
            SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
            yield return new WaitForSeconds(0.2f);

            if (gameManager.instance.currentScene.buildIndex != credits)
                SoundManager.Instance.StopMusic();
            
            SceneManager.LoadScene(lvl);

            
        }
        else if (lvl != company)
        {
            SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadScene(lvl);

        }
        else
        {
            SceneManager.LoadScene(lvl);
        }

    }


}
