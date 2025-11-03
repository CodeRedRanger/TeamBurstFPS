using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; 



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
    public AudioClip winMusic;

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

    //Kids rescued for lunchroom
    int kidsRescued;
    [SerializeField] TMP_Text kidsRescuedText;

    //Level 0: Main Menu; Level 1: Playground; Level 2: Hall/Library; Level 3: Credits; Level 4: Alien Ship
    //Level 5: Lunchroom, Level 6: Launchpad, Level 7: Company. 
    [HideInInspector] public bool Level1, Level2, Level3, LevelLunch, LevelLaunchpad, LevelCompany;

    public TMP_Text ammoCur, ammoMax;
    public TMP_Text hotBarSlot1, hotbarSlot2, hotbarSlot3;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;
    public GameObject speedboostPopup;
    public GameObject jumpboostPopup;
    public GameObject doublejumpPopup;
    public GameObject invinciblePopup; 
    public GameObject bombPopup;
    public GameObject grenadePopup;
    public GameObject stunnerPopup;

    [HideInInspector] public bool flashBombUI = false;
    [HideInInspector] public bool flashGrenadeUI = false;
    [HideInInspector] public bool flashStunnerUI = false;

    [HideInInspector] public bool enableBomb = false;
    [HideInInspector] public bool enableGrenade = false;
    [HideInInspector] public bool enableStunner = false;

    public GameObject runPopup;
    public GameObject kidsPopup;


    //from main menu, you don't need the actions of unpause the first time, even though it is
    //called as part of load scene. 
    [HideInInspector] public bool firstUnpause = true;

    [HideInInspector] public Scene currentScene;

    
   

    void Awake()
    {

        instance = this;
        timeScaleOrig = Time.timeScale;

        
        currentScene = SceneManager.GetActiveScene();

        //Need if statement so this doesn't fire during main menu
        //But is needed if testing, starting from level 1

        if (currentScene.buildIndex == 0 || currentScene.buildIndex == 3)
        {
            statePause(); 
        }

        if (currentScene.buildIndex != 0 && currentScene.buildIndex != 3)
        {
            //need this line before next
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
            if (SoundManager.Instance != null ) 
            SoundManager.Instance.PlayMusic(BGMusic);
            playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
            firstUnpause = false;
          
            //Playground
            if (currentScene.buildIndex == 1)
            {
                Level1 = true;
                Level2 = false;
                Level3 = false;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = false;
            }

            //Hall/Library
            if (currentScene.buildIndex == 2)
            {
                Level1 = false;
                Level2 = true;
                Level3 = false;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = false;
                StartCoroutine(FlashRunUI()); 
            }

            //Alien Ship
            if (currentScene.buildIndex == 4)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = false;
            }

            if (currentScene.buildIndex == 5)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = true;
                LevelLaunchpad = false;
                LevelCompany = false;
                StartCoroutine(FlashKidsUI());
            }

            if (currentScene.buildIndex == 6)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = false;
                LevelLaunchpad = true;
                LevelCompany = false;
            }

            if (currentScene.buildIndex == 7)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = true;
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
       
        isPaused = !isPaused;

        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //normal pause; first unpause true if new scene being loaded
        if (firstUnpause == false)
         {
             menuActive.SetActive(false);
             menuActive = null;
             SoundManager.Instance.PlayMusic(BGMusic);
         }
            
         firstUnpause = false;
     

    }

    public void updateKidsRescued(int amount)
    {
        kidsRescued += amount;
        kidsRescuedText.text = kidsRescued.ToString("F0");



        if (kidsRescued <= 0)
        {
            //win condition
            if (LevelLunch == true)
            {
                //statePause();
                //menuActive = menuWin;
                //menuActive.SetActive(true);
                //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
                SoundManager.Instance.PlayEffect(winMusic, 1);
                youWinEnd();
            }

            //can't get to work
            //SoundManager.Instance.ChangeVolumeMusic(0.3f);

        }
    }




    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString("F0");



        if (gameGoalCount <= 0)
        {
            //win condition
            if (Level2 == true)
            {
                //statePause();
                //menuActive = menuWin;
                //menuActive.SetActive(true);
                //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
                SoundManager.Instance.PlayEffect(winMusic, 1);
                youWinEnd(); 
            }

            if (Level1 == true)
            { 
                SoundManager.Instance.PlayEffect(toSchool, 1);
            } 



            //can't get to work
            //SoundManager.Instance.ChangeVolumeMusic(0.3f);

        }
    }

    public int GetKidsRescued()
    {
        return kidsRescued;
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

    public void flashItemUI()
    {
        StartCoroutine(PowerupFeedback()); 
    }
    public IEnumerator PowerupFeedback()
   {
        if (flashBombUI)
        {
            flashBombUI = false;
            bombPopup.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            bombPopup.SetActive(false);
        }
        if (flashGrenadeUI)
        {
            flashGrenadeUI = false;
            grenadePopup.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            grenadePopup.SetActive(false);
        }
        if (flashStunnerUI)
        {
            flashStunnerUI = false;
            stunnerPopup.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            stunnerPopup.SetActive(false);
        }
    }

    public IEnumerator FlashRunUI()
    {
         runPopup.SetActive(true);
         yield return new WaitForSeconds(3.0f);
         runPopup.SetActive(false);
        
    }

    public IEnumerator FlashKidsUI()
    {
        kidsPopup.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        kidsPopup.SetActive(false);

    }

}
