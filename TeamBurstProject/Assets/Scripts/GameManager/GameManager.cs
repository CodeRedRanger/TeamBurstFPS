using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




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
    [SerializeField] GameObject firstSelectedLose;
    private bool loseMenu = false;
    public GameObject firstSelectedEnd; 
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject hotBar;
    public GameObject firstSelectedMain;
    public GameObject firstSelectedOptions;
    public GameObject firstSelectedCredits;



    //Sound variables
    float currentMusicVolume; 
    private AudioClip BGMusic;
    public AudioClip toSchool;
    public AudioClip run;
    private bool kidsSpawned = false;
    public AudioClip thankYou;
    public AudioClip winMusic;
    private bool startMusic = false; 

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
    [HideInInspector] public bool isPaused;

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

        //ORIGINAL VERSION
        //instance = this;

        /* RECENT VERSION
        if (instance != null)
        {
            Destroy(gameObject);
            //return;

        }
        else
        {
            instance = this;
        }*/

        /*
        if (eventSystem == null)
        {
            eventSystem = GetComponentInChildren<EventSystem>();
        }*/


        LoadItemStatus("Bomb");
        GivePlayerItems(); 
        timeScaleOrig = Time.timeScale;
        currentScene = SceneManager.GetActiveScene();

        //Need if statement so this doesn't fire during main menu
        //But is needed if testing, starting from level 1

        if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits || currentScene.buildIndex == options
            || currentScene.buildIndex == company)
        {

            statePause();

            if (currentScene.buildIndex == mainMenu)
            {
                //reset you items
                enableBomb = false;
                SaveItemStatus("Bomb", false);
                EventSystem.current.SetSelectedGameObject(firstSelectedMain);

                //SoundManager.Instance.PlayEffect(MainMenuSFX, 1);
                //if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEffectDelayed(MainMenuSFX, 1, 0.5f); 
            }

            if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits) // || currentScene.buildIndex == 7)
            {
                //if (SoundManager.Instance != null)
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
                //reset items for all levels (reset in each level below, the items you would get on those levels)
                enableBomb = false;
                SaveItemStatus("Bomb", false); 
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
            {
                //added this to change back volume when level changed
                float currentMusicValue = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
                float currentMusicVolume = Mathf.Log10(currentMusicValue) * 30f;
                SoundManager.Instance.masterMixer.SetFloat("MusicVolume", currentMusicVolume);
                SoundManager.Instance.PlayMusic(BGMusic);
            }
            else
            {
                startMusic = true; 
            }



        }
        


    }

    void Update()
    {

        if (startMusic)
        {
            float currentMusicValue = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
            float currentMusicVolume = Mathf.Log10(currentMusicValue) * 30f;
            SoundManager.Instance.masterMixer.SetFloat("MusicVolume", currentMusicVolume);
            SoundManager.Instance.PlayMusic(BGMusic);
            startMusic = false; 
        }

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
        else if (loseMenu)
            EventSystem.current.SetSelectedGameObject(firstSelectedLose);
        else
            EventSystem.current.SetSelectedGameObject(firstSelectedPause);

        continueMenu = false;
        endMenu = false;
        loseMenu = false;

        if (currentScene.buildIndex != mainMenu && currentScene.buildIndex != credits)
        //&& currentScene.buildIndex != 7)&& currentScene.buildIndex != 8)
        {

            if (SoundManager.Instance != null)
            {
                //SoundManager.Instance.StopMusic();
                
                currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);

                //need to only change volume if scene is the same; need to stop it if going to a new scene
                //SoundManager.Instance.ChangeVolumeMusic(0.2f);
                SoundManager.Instance.LowerVolumeInstantly(); 
                PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
            }
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

        //Can load settings for guns and items here, put in Awake
        if (firstUnpause)
        {
            //LoadItemStatus("Bomb"); 
        }

        //normal pause; first unpause true if new scene being loaded
        if (firstUnpause == false)
         {

            

            menuActive.SetActive(false);
             menuActive = null;
            //SoundManager.Instance.PlayMusic(BGMusic);
            //only need to raise volume if unpausing in same scene, need to play new music if changing scenes
            SoundManager.Instance.ChangeVolumeMusic(currentMusicVolume);


        }
            
         firstUnpause = false;
     

    }


    public void SaveItemStatus(string itemName, bool hasItem)
    {
        int saveValue = hasItem ? 1 : 0;
        PlayerPrefs.SetInt("Has_" + itemName, saveValue);
        PlayerPrefs.Save();


        /*
         bool enableBomb = false;
         bool enableGrenade = false;
         bool enableStunner = false;
         inventory item 1, 2 and 3 (some might be null)
         
         */

    }

    public bool LoadItemStatus(string itemName)
    {
        int savedValue = PlayerPrefs.GetInt("Has_" + itemName, 0);

        return savedValue == 1;

    }

    public void GivePlayerItems()
    {


        if (LoadItemStatus("Bomb"))
        {
            //update hotbar UI
            enableBomb = true;
        }
        if (LoadItemStatus("Grenade"))
        {
            //update hotbar UI
            //enableGrenade = true
        }

        if (LoadItemStatus("Stunner"))
        {
            //update hotbar UI
            //enableStunner = true
        }

        if (LoadItemStatus("Pistol"))
        {
            //add the pistol to inventory through script?
        }

        if (LoadItemStatus("SMGun"))
        {
            //add the medkit to inventory through script?
        }

        if (LoadItemStatus("Cannon"))
        {
            //add the rifle to inventory through script?

        }
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
                youWin();
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
                youWin(); 
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
      
       

        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);

        //need to only change volume if scene is the same; need to stop it if going to a new scene
        //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
        //SoundManager.Instance.ChangeVolumeMusic(0.2f);
        SoundManager.Instance.LowerVolumeInstantly();
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);

    }

    public void youWinEnd()
    {
        //win condition
        endMenu = true;
        statePause();
        menuActive = menuWinEnd;
        menuActive.SetActive(true);

        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);

        //need to only change volume if scene is the same; need to stop it if going to a new scene
        //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
        //SoundManager.Instance.ChangeVolumeMusic(0.2f);
        SoundManager.Instance.LowerVolumeInstantly();
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
    }

    public void youLose()
    {
        loseMenu = true;
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);

        //currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);

        //need to only change volume if scene is the same; need to stop it if going to a new scene
        //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
        //SoundManager.Instance.ChangeVolumeMusic(0.2f);


        //SoundManager.Instance.LowerVolumeInstantly();
        //PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);

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
