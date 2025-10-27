using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    Scene currentScene; 
    public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void restartLevel()
    {
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
           Application.Quit(); //note: won't work in editor, only in build  
#endif
    }

    public void respawn()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();    
    }

    public void loadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
        gameManager.instance.firstUnpause = true;
        gameManager.instance.stateUnpause(); //in case paused when changing level
       
    }

}
