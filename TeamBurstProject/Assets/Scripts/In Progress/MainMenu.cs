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
        instance = this;
        currentScene = SceneManager.GetActiveScene();
        state = GameState.MainMenu;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene.buildIndex != 0)
        {
            state = GameState.Gameplay;
        }

    }
}
