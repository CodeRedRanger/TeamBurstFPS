using Unity.VisualScripting;
using UnityEngine;

/
public class gameManager : MonoBehaviour


{
    public static gameManager instance;
    //any open menu will go into menuActive and then close active menu 
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public GameObject player; //reference to player object
    public PlayerController playerScript; //reference to player script

    //could use getter and setter
    public bool isPaused;

    //when paused, timeScale is 0, when unpaused, timeScale is 1
    //input won't work and enemies won't move when timeScale is 0
    float timeScaleOrig;

    int gameGoalCount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        //need this line before next
       player = GameObject.FindGameObjectWithTag("Player");
       playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel")) //cancel is escape key by default
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);

                //if pause menu has options, then pause menue is an array (pause, settings, audio, etc)
                //escape goes back through the array backwards to close all submenus first
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }

    }

    public void statePause()
    {

        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;



    }

    public void stateUnpause()
    {

        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        if (gameGoalCount <= 0)
        {
            //win condition
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);

        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);

    }

}
