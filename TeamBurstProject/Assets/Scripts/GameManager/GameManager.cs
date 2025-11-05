using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;



public class gameManager : MonoBehaviour
{

    public static gameManager instance;
    public EventSystem eventSystem; 
    //any open menu will go into menuActive and then close active menu 
    //Menu variables
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    public GameObject firstSelectedPause; //first selected button in pause menu
    [SerializeField] GameObject menuWin;
    private bool continueMenu = false; 
    public GameObject firstSelectedContinue; 
    [SerializeField] GameObject menuWinEnd;
    private bool endMenu = false;
    public GameObject firstSelectedEnd; 
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject hotBar;
    public GameObject firstSelectedMain;
    public GameObject firstSelectedOptions;
    public GameObject firstSelectedCredits; 



    //Sound variables
    private AudioClip BGMusic;
    public AudioClip toSchool;
    public AudioClip run;
    private bool kidsSpawned = false;
    public AudioClip thankYou;
    public AudioClip winMusic;

    [SerializeField] AudioClip MainMenuSFX;
    [SerializeField] AudioClip MainMenuMusic;
    [SerializeField] AudioClip PlaygroundMusic;
    [SerializeField] AudioClip LibraryMusic;
    [SerializeField] AudioClip LunchroomMusic;
    [SerializeField] AudioClip LaunchpadMusic;
    [SerializeField] AudioClip AlienshipMusic;

    

    public GameObject player; //reference to player object
    public PlayerController playerScript; //reference to player script

    //could use getter and setter
    public bool isPaused;

    //when paused, timeScale is 0, when unpaused, timeScale is 1
    //input won't work and enemies won't move when timeScale is 0
    float timeScaleOrig;

    //Game goal variables
    int gameGoalCount;

    //Kids rescued for lunchroom
    int kidsRescued;
    [SerializeField] TMP_Text kidsRescuedText;

    //Level 0: Main Menu; Level 1: Playground; Level 2: Hall/Library; Level 3: Credits; Level 4: Alien Ship
    //Level 5: Lunchroom, Level 6: Launchpad, Leve 7: Options, Level 8: Company
    //Rearrange all levels so that 0 is company, 1 is main menu, 2 is options, 3 is credits
    //4 is playground, 5 is hall/library, 6 is lunchroom, 7 is launchpad, 8 is alien ship
    //Will have to change all instances in this script and other scripts for currentScene.buildIndex
    [HideInInspector] public bool Level1, Level2, Level3, LevelLunch, LevelLaunchpad, LevelOptions, LevelCompany;
    private int mainMenu = 0;
    private int playground = 1;
    private int library = 2;
    private int lunchroom = 5;
    private int launchpad = 6;
    private int alienship = 4;
    private int credits = 3;
    private int options = 7;
    private int company = 8;

    //HUD variables
    [SerializeField] TMP_Text gameGoalCountText;
    public TMP_Text ammoCur, ammoMax;
    public TMP_Text hotBarSlot1, hotbarSlot2, hotbarSlot3;
    public Image playerHPBar;
    public Image playerHPBarUp;
    public Image playerHPBarDown;
    public GameObject playerDamageFlash;

    //Spawn point variables 
    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;

    //Powerup variables
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

    //Level specific popups
    public GameObject runPopup;
    public GameObject kidsPopup;


    //from main menu, you don't need the actions of unpause the first time, even though it is
    //called as part of load scene. 
    [HideInInspector] public bool firstUnpause = true;

    [HideInInspector] public Scene currentScene;

    
   

    void Awake()
    {
        if (instance == null)
        {
            /*
            if (transform.parent != null)
            {
                transform.parent = null;
            }*/

            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
        //instance = this;

        /*
        if (instance != null)
        {
            Destroy(gameObject);
            //return;

        }
        else
        {
            instance = this;
        }*/

        if (eventSystem == null)
        {
            eventSystem = GetComponentInChildren<EventSystem>();
        }


            timeScaleOrig = Time.timeScale;

        
        currentScene = SceneManager.GetActiveScene();

        //Need if statement so this doesn't fire during main menu
        //But is needed if testing, starting from level 1

        if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits || currentScene.buildIndex == options
            || currentScene.buildIndex == company)
        {

            statePause(); 

            if(currentScene.buildIndex == mainMenu)
            {
                EventSystem.current.SetSelectedGameObject(firstSelectedMain);
                //SoundManager.Instance.PlayEffect(MainMenuSFX, 1);
                SoundManager.Instance.PlayEffectDelayed(MainMenuSFX, 1, 0.5f); 
            }

            if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits) // || currentScene.buildIndex == 7)
            {
                if (!SoundManager.Instance.MusicIsPlaying())
                {
                    SoundManager.Instance.LoadVolumes();
                    //if (SoundManager.Instance.masterMixer.GetFloat("MasterVolume", out float volume))
                    //    Debug.Log($"Current VOLUME of '{"MasterVolume"}' is {volume} dB");
                    
                    float value = PlayerPrefs.GetFloat("MusicVolume", 1);
                    //Debug.Log($"Current VOLUME of '{"MusicVolume"}' is {value}");
                    SoundManager.Instance.PlayMusic(MainMenuMusic);
                }
            }

            if (currentScene.buildIndex == credits)
            {
                EventSystem.current.SetSelectedGameObject(firstSelectedCredits);
            }
            else if (currentScene.buildIndex == options)
            {
                EventSystem.current.SetSelectedGameObject(firstSelectedOptions);
            }


        }

        //later change to 0, 1, 2, 3 (when make company, main menu, options, credits 0,1,2,3)
        if (currentScene.buildIndex != mainMenu && currentScene.buildIndex != credits && currentScene.buildIndex != options 
            && currentScene.buildIndex != company)
        {
            //need this line before next
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
           
            playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
            firstUnpause = false;
          
            //Playground
            if (currentScene.buildIndex == playground)
            {
                Level1 = true;
                Level2 = false;
                Level3 = false;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = false;
                BGMusic = PlaygroundMusic;
            }

            //Hall/Library
            if (currentScene.buildIndex == library)
            {
                Level1 = false;
                Level2 = true;
                Level3 = false;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = false;
                BGMusic = LibraryMusic;
                StartCoroutine(FlashRunUI()); 
            }

            //Alien Ship
            if (currentScene.buildIndex == alienship)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = false;
                LevelLaunchpad = false;
                LevelCompany = false;
                BGMusic = AlienshipMusic;
            }

            if (currentScene.buildIndex == lunchroom)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = true;
                LevelLaunchpad = false;
                LevelCompany = false;
                BGMusic = LunchroomMusic;
                StartCoroutine(FlashKidsUI());
            }

            if (currentScene.buildIndex == launchpad)
            {
                Level1 = false;
                Level2 = false;
                Level3 = true;
                LevelLunch = false;
                LevelLaunchpad = true;
                LevelCompany = false;
                BGMusic = LaunchpadMusic;
            }


            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMusic(BGMusic);



        }
        


    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        
    
        if (currentScene.buildIndex != mainMenu && currentScene.buildIndex != credits && currentScene.buildIndex != options
            && currentScene.buildIndex != company)
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
        //ADDED THIS
        //AudioListener.pause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (continueMenu)
            EventSystem.current.SetSelectedGameObject(firstSelectedContinue);
        else if (endMenu)
            EventSystem.current.SetSelectedGameObject(firstSelectedEnd);
        else
            EventSystem.current.SetSelectedGameObject(firstSelectedPause);

        continueMenu = false;
        endMenu = false;

        if (currentScene.buildIndex != mainMenu && currentScene.buildIndex != credits)
        //&& currentScene.buildIndex != 7)&& currentScene.buildIndex != 8)
        {

            if (SoundManager.Instance != null)
                SoundManager.Instance.StopMusic();
        }
        
    }

    public void stateUnpause()
    {
        //normal pause
       
        isPaused = !isPaused;

        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        EventSystem.current.SetSelectedGameObject(null);

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
       
        StartCoroutine(WaitForKidsToSpawn());
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
        continueMenu = true;
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
        SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
    }

    public void youWinEnd()
    {
        //win condition
        endMenu = true;
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
        SoundManager.Instance.PlayEffect(run, 1); 
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

    public IEnumerator WaitForKidsToSpawn()
    {
        if (!kidsSpawned)
            yield return new WaitForSeconds(2.0f);
        else
            SoundManager.Instance.PlayEffect(thankYou, 1);
        kidsSpawned = true;
    }


}
