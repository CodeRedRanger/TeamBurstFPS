using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class gameManager : MonoBehaviour
{

    public static gameManager instance;
    //any open menu will go into menuActive and then close active menu 
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuWinEnd;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject hotBar; 
    [SerializeField] TMP_Text gameGoalCountText;

    public AudioClip BGMusic;
    public AudioClip toSchool;

    public Image playerHPBar;
    public GameObject playerDamageFlash; 

    public GameObject player; //reference to player object
    public PlayerController playerScript; //reference to player script

    //could use getter and setter
    public bool isPaused;

    //when paused, timeScale is 0, when unpaused, timeScale is 1
    //input won't work and enemies won't move when timeScale is 0
    float timeScaleOrig;

    int gameGoalCount;
    public bool Level1 = true;

    public TMP_Text ammoCur, ammoMax;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;

    
    void Awake()
    {

        instance = this;
        timeScaleOrig = Time.timeScale;

        //need this line before next
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        SoundManager.Instance.PlayMusic(BGMusic);
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        

    }

    // Update is called once per frame
    void Update()
    {
        if (MainMenu.instance.state == GameState.Gameplay)
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

            if (Input.GetButtonDown("HotBar"))
            {
                if (hotBar.activeSelf == true)
                {
                    hotBar.SetActive(false);
                }
                else if (hotBar.activeSelf == false)
                {
                    hotBar.SetActive(true);
                }
            }

        }

        else
        {
            statePause(); 
        }

    }

    public void statePause()
    {

        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SoundManager.Instance.StopMusic();



    }

    public void stateUnpause()
    {

        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(MainMenu.instance.state == GameState.MainMenu)
        {
            menuActive = menuPause;
            MainMenu.instance.state = GameState.Gameplay; 
        }

        menuActive.SetActive(false);
        menuActive = null;
        SoundManager.Instance.PlayMusic(BGMusic);
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString("F0");



        if (gameGoalCount <= 0)
        {
            //win condition
            //statePause();
            //menuActive = menuWin;
            //menuActive.SetActive(true);
            //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);

            if (Level1 == true)
            { 
                SoundManager.Instance.PlayEffect(toSchool, 1);
            } 



            //can't get to work
            //SoundManager.Instance.ChangeVolumeMusic(0.3f);

        }
    }

    public int GetGameGoalCount()
    {
        return gameGoalCount;
    }

    public void youWin()
    {
        //win condition
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
        SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
    }

    public void youWinEnd()
    {
        //win condition
        statePause();
        menuActive = menuWinEnd;
        menuActive.SetActive(true);
        SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
        SoundManager.Instance.PlayMusic(BGMusic, 0.2f);

        //can't get to work
        //SoundManager.Instance.ChangeVolumeMusic(0.3f); 

    }

}
