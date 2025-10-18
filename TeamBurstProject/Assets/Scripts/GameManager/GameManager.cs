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
    public Image playerHPBarUp;
    public Image playerHPBarDown;
    public GameObject playerDamageFlash;

    public GameObject player; //reference to player object
    public PlayerController playerScript; //reference to player script

    //could use getter and setter
    public bool isPaused;

    //when paused, timeScale is 0, when unpaused, timeScale is 1
    //input won't work and enemies won't move when timeScale is 0
    float timeScaleOrig;

    int gameGoalCount;
    public bool Level1;

    public TMP_Text ammoCur, ammoMax;
    public TMP_Text hotBarSlot1, hotbarSlot2, hotbarSlot3;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;
    public GameObject speedboostPopup;
    public GameObject jumpboostPopup;
    public GameObject doublejumpPopup;


    //from main menu, you don't need the actions of unpause the first time, even though it is
    //called as part of load scene. 
    private bool firstUnpause = true;

    public Scene currentScene;

    public bool enableBomb = false;
    public bool enableGrenade = false;
    public bool enableStunner = false;
   

    void Awake()
    {

        instance = this;
        timeScaleOrig = Time.timeScale;

        
        currentScene = SceneManager.GetActiveScene();

        //Need if statement so this doesn't fire during main menu
        //But is needed if testing, starting from level 1
        if (currentScene.buildIndex != 0)
        {
            //need this line before next
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
            if (SoundManager.Instance != null ) 
            SoundManager.Instance.PlayMusic(BGMusic);
            playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
            firstUnpause = false;
          

            if (currentScene.buildIndex == 1)
            {
                Level1 = true;
            }

        }
        


    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        
    
        if (currentScene.buildIndex != 0) 
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
        //normal pause
        if (firstUnpause == false)
        {
            isPaused = !isPaused;

            Time.timeScale = timeScaleOrig;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            menuActive.SetActive(false);
            menuActive = null;
            SoundManager.Instance.PlayMusic(BGMusic);
        }
        //pause out of main menu
        else
        { 
            firstUnpause = false;
        }
          
            


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
