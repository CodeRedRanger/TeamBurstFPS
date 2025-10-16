using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Gameplay, Settings }
   
    public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public GameState state; 
    public Scene currentScene; 
    void Awake()
    {
        if (instance != null && instance != this)
        {
            //so only one instance of MainMenu exists at any time
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            currentScene = SceneManager.GetActiveScene();
            state = GameState.MainMenu;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        //not needed because as soon as you push start, unpause is called which sets gamestate to gameplay
        /*
        if (currentScene.buildIndex != 0)
        {
            state = GameState.Gameplay;
        }*/

    }
}
