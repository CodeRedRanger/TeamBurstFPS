using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;





public class ButtonFunctions : MonoBehaviour
{
    //make enum later
    private int mainMenu = 1;
    private int playground = 2;
    //private int library = 3;
    //private int lunchroom = 4;
    //private int launchpad = 5;
    //private int alienship = 6;
    private int credits = 7;
    //private int options = 8;
    private int company = 0;

    //below is not needed. You can just call current scene from game manager
    Scene currentScene;
    [SerializeField] AudioClip nonStartButtonSound;
    [SerializeField] AudioClip StartButtonSound;

    void Update()
    {
        
        if(gameManager.instance.currentScene.buildIndex == company && Input.GetButtonDown("Cancel"))
        {
            loadLevel(mainMenu);
        }


    }


    public void resume()
    {
        SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
        gameManager.instance.stateUnpause();
    }

    public void restartLevel()
    {
        //resume looping after win screens
        SoundManager.Instance.musicSource.loop = true;
        SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
        //reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //load items from beginning of level here
        gameManager.instance.LoadItemStatus(gameManager.instance.bomb);
        gameManager.instance.LoadItemStatus(gameManager.instance.grenade);
        gameManager.instance.LoadItemStatus(gameManager.instance.stunner);
        gameManager.instance.LoadItemStatus(gameManager.instance.pistol);
        gameManager.instance.LoadItemStatus(gameManager.instance.smgun);
        gameManager.instance.LoadItemStatus(gameManager.instance.cannon);
        gameManager.instance.LoadItemStatus(gameManager.instance.flamethrower);
        gameManager.instance.GivePlayerItems();


        gameManager.instance.firstUnpause = true;
        gameManager.instance.stateUnpause(); //in case paused when restarting level
    }

    public void quit()
    {
        //reset you items
        gameManager.instance.ResetAllItems(); 

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

        //Coroutine to wait for end song to finish before loading next level
        loadLevel(lvl);
    }


    public void loadLevel(int lvl)
    {
        //resume looping after win screens
        SoundManager.Instance.musicSource.loop = true;

        //save your items
        gameManager.instance.SaveItemStatus(gameManager.instance.bomb, gameManager.instance.enableBomb);
        gameManager.instance.SaveItemStatus(gameManager.instance.grenade, gameManager.instance.enableGrenade);
        gameManager.instance.SaveItemStatus(gameManager.instance.stunner, gameManager.instance.enableStunner);

        gameManager.instance.SaveItemStatus(gameManager.instance.pistol, gameManager.instance.hasPistol);  
        gameManager.instance.SaveItemStatus(gameManager.instance.smgun, gameManager.instance.hasSMG);
        gameManager.instance.SaveItemStatus(gameManager.instance.cannon, gameManager.instance.hasCannon);
        gameManager.instance.SaveItemStatus(gameManager.instance.flamethrower, gameManager.instance.hasFlameThrower);

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
        //check if game paused
        //if game unpaused, pause (then unpause after wait), do this only for continue button. 


        if (lvl == playground)
        {
            SoundManager.Instance.PlayEffect(StartButtonSound, 1f);
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(lvl);

        }
        else if (lvl == mainMenu)
        {

            if (gameManager.instance.currentScene.buildIndex != company)
            {
                SoundManager.Instance.PlayEffect(nonStartButtonSound, 1f);
                yield return new WaitForSeconds(0.2f);
            }

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
