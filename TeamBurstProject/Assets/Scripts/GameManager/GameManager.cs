using System.Collections;
using System.Collections.Generic;
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
    public GameObject backButton; 



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

    //credits screen
    [SerializeField] public GameObject assetsPopup;

    //library variable
    [HideInInspector] public bool libraryCheckpoint = false;

    //Kids rescued for lunchroom
    int kidsRescued;
    [SerializeField] TMP_Text kidsRescuedText;


    //Keys collected for lunchroom level
    public GameObject tryDoorPopup;
    [HideInInspector] public int keysFor3KeyDoor = 3;
    public TMP_Text keysFor3KeyDoorText;

    //Keys collected for launchpad level
    public GameObject keysCollected;
    [HideInInspector] public int keysRequired; //set below in Awake for launchpad level
    [HideInInspector] public int keysCount;
    [SerializeField] TMP_Text keysCollectedText;
    public GameObject moreKeysNeededPopup;
    //For launchpad boss
    [HideInInspector] public bool launchpadBossKilled;
    


    //Soldiers killed for alien spaceship
    [HideInInspector] public int soldiersToKill;
    [SerializeField] GameObject finalDoor; 

    //Level 0: Main Menu; Level 1: Playground; Level 2: Hall/Library; Level 3: Credits; Level 4: Alien Ship
    //Level 5: Lunchroom, Level 6: Launchpad, Leve 7: Options, Level 8: Company
    //Rearrange all levels so that 0 is company, 1 is main menu, 2 is options, 3 is credits
    //4 is playground, 5 is hall/library, 6 is lunchroom, 7 is launchpad, 8 is alien ship
    //Will have to change all instances in this script and other scripts for currentScene.buildIndex
    [HideInInspector] public bool Level1, Level2, Level3, LevelLunch, LevelLaunchpad, LevelOptions, LevelCompany;
    private int mainMenu = 1;
    private int playground = 2;
    private int library = 3;
    private int lunchroom = 4;
    //launch pad 6 to 4
    private int launchpad = 5;
    //alienship 4 to 5
    private int alienship = 6;
    //credits 5 to 6
    private int credits = 7;
    private int options = 8;
    private int company = 0;

    //HUD variables
    [SerializeField] TMP_Text gameGoalCountText;
    public TMP_Text ammoCur, ammoMax;
    public TMP_Text hotBarSlot1, hotbarSlot2, hotbarSlot3;
    public Image playerHPBar;
    public Image playerHPBarUp;
    public Image playerHPBarDown;
    public GameObject playerDamageFlash;
    public GameObject playerInvincibleFlash;
    public GameObject bossHPBar;

    //Spawn point variables 
    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;

    //Interactables variables
    public GameObject leverPopup; 

    //Powerup variables
    public GameObject speedboostPopup;
    public GameObject jumpboostPopup;
    public GameObject doublejumpPopup;
    public GameObject invinciblePopup;
    public GameObject jetpackPopup;
    public GameObject gravityBootsPopup; 
    public GameObject bombPopup;
    public GameObject grenadePopup;
    public GameObject stunnerPopup;

    [HideInInspector] public bool flashBombUI = false;
    [HideInInspector] public bool flashGrenadeUI = false;
    [HideInInspector] public bool flashStunnerUI = false;

    [HideInInspector] public bool enableBomb = false;
    [HideInInspector] public bool enableGrenade = false;
    [HideInInspector] public bool enableStunner = false;

    [SerializeField] public ItemData bombObj;
    [SerializeField] public ItemData grenadeObj;
    [SerializeField] public ItemData stunnerObj;

    [HideInInspector] public string bomb = "Bomb";
    [HideInInspector] public string grenade = "Grenade";
    [HideInInspector] public string stunner = "Stunner";
    [HideInInspector] public string pistol = "Pistol";
    [HideInInspector] public string smgun = "SMGun";
    [HideInInspector] public string cannon = "Cannon";
    [HideInInspector] public string flamethrower = "Flamethrower";

    [HideInInspector] public bool hasPistol = false;
    [HideInInspector] public bool hasSMG = false;
    [HideInInspector] public bool hasCannon = false;
    [HideInInspector] public bool hasFlameThrower = false;


    [SerializeField] GunData pistolObj;
    [SerializeField] GunData smgObj;
    [SerializeField] GunData cannonObj;
    [SerializeField] GunData flamethrowerObj;

    // Reticle stuff
    [SerializeField] public GameObject reticle;
    [SerializeField] private Image ammoIcon;
    [SerializeField] Reticle reticleScript;
    [SerializeField] public GameObject rechargePopup; 




    //Level specific popups
    public GameObject runPopup;
    public GameObject kidsPopup;
    public GameObject launchpadPopup;


    

    //from main menu, you don't need the actions of unpause the first time, even though it is
    //called as part of load scene. 
    [HideInInspector] public bool firstUnpause = true;
    [HideInInspector] public bool fromContinueMenu = false;

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


        timeScaleOrig = Time.timeScale;
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == company)
        {
            ResetAllItems();
        }


        //Need if statement so this doesn't fire during main menu
        //But is needed if testing, starting from level 1

        if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits || currentScene.buildIndex == options)
            //|| currentScene.buildIndex == company) //Must have delta time for company menu for fade in
        {

            
            statePause();

            if (currentScene.buildIndex == mainMenu)
            {
                //reset you items
                ResetAllItems(); 

                EventSystem.current.SetSelectedGameObject(firstSelectedMain);

                //SoundManager.Instance.PlayEffect(MainMenuSFX, 1);
                //if (SoundManager.Instance != null)
                //    SoundManager.Instance.PlayEffectDelayed(MainMenuSFX, 1, 0.5f);
               
                
            }

            if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits) // || currentScene.buildIndex == 7)
            {
                BGMusic = MainMenuMusic;
                if (SoundManager.Instance != null)
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
                ResetAllItems();

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
                keysCollected.SetActive(true);
                keysCount = 0;
                keysRequired = 5; //UI updated below in UpdateKeysCollected
                StartCoroutine(FlashLaunchpadUI());
            }

            /*
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
            */


        }

        if (SoundManager.Instance != null)
        {
            
            //added this to change back volume when level changed on pause or win/lose menu
            float currentMusicValue = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
            float currentMusicVolume = Mathf.Log10(currentMusicValue) * 30f;
            SoundManager.Instance.masterMixer.SetFloat("MusicVolume", currentMusicVolume);

            if (currentScene.buildIndex != mainMenu && currentScene.buildIndex != credits && currentScene.buildIndex != options)
            {
                if (BGMusic != null)
                {
                    SoundManager.Instance.PlayMusic(BGMusic);
                }
            }

        }
        else
        {
            startMusic = true;
        }

        LoadItemStatus(bomb);
        LoadItemStatus(grenade);
        LoadItemStatus(stunner);
        LoadItemStatus(pistol);
        LoadItemStatus(smgun);
        LoadItemStatus(cannon);
        LoadItemStatus(flamethrower);
        GivePlayerItems();

    }

    private void Start()
    {
        if (currentScene.buildIndex == mainMenu)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEffectDelayed(MainMenuSFX, 1, 0.5f);
        }

        /*
        if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits || currentScene.buildIndex == options
           || currentScene.buildIndex == company)
        {

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEffectDelayed(MainMenuSFX, 1, 0.5f);

            if (currentScene.buildIndex == mainMenu || currentScene.buildIndex == credits)
            {
                if (SoundManager.Instance != null)

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
            }
        }*/



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
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")) //cancel is escape key by default
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
        reticle.SetActive(false);
        reticleScript.Hide();

        if (continueMenu)
            EventSystem.current.SetSelectedGameObject(firstSelectedContinue);
        else if (endMenu)
            EventSystem.current.SetSelectedGameObject(firstSelectedEnd);
        else if (loseMenu)
            EventSystem.current.SetSelectedGameObject(firstSelectedLose);
        else
            EventSystem.current.SetSelectedGameObject(firstSelectedPause);

        //continueMenu = false;
        //endMenu = false;
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
                if (!continueMenu && !endMenu)
                {
                    SoundManager.Instance.LowerVolumeInstantly();
                    PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
                }
                
            }
        }

        continueMenu = false;
        endMenu = false;

    }

    public void stateUnpause()
    {
        //normal pause
       
        isPaused = !isPaused;

        if (player != null)
        {
            if(playerScript.gunList.Count > 0)
            {
                reticle.SetActive(true); 
                reticleScript.Refresh();
            }
        }

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

    }

    public bool LoadItemStatus(string itemName)
    {
        int savedValue = PlayerPrefs.GetInt("Has_" + itemName, 0);

        return savedValue == 1;

    }

    public void GivePlayerItems()
    {


        if (LoadItemStatus(bomb))
        {
            //update hotbar UI

            /*
            if(InventoryManager.Instance != null && bombObj != null)
            {
                InventoryManager.Instance.AddItem(bombObj);
            }*/

            enableBomb = true;
        }
        if (LoadItemStatus(grenade))
        {
            //update hotbar UI
            enableGrenade = true; 
        }

        if (LoadItemStatus(stunner))
        {
            //update hotbar UI
            enableStunner = true; 
        }

        
        if (LoadItemStatus(pistol))
        {
            GunData gun = pistolObj; 
            playerScript.gunList.Add(gun);
            hasPistol = true;
            
        }
        
        if (LoadItemStatus(smgun))
        {
            GunData gun = smgObj;
            playerScript.gunList.Add(gun);
            hasSMG = true;
        }
       
        if (LoadItemStatus(cannon))
        {
            GunData gun = cannonObj;
            playerScript.gunList.Add(gun);
            hasCannon = true; 
        }

        if (LoadItemStatus(flamethrower))
        {
            GunData gun = flamethrowerObj;
            playerScript.gunList.Add(gun);
            hasFlameThrower = true;
        }

  
        if (playerScript != null && playerScript.gunList.Count > 0)
        {
            //try deleting below
            //playerScript.gunListPos = 0;
            //try changing position to 0
            //playerScript.gunModel.GetComponent<MeshFilter>().sharedMesh = playerScript.gunList[playerScript.gunListPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
            //playerScript.gunModel.GetComponent<MeshRenderer>().sharedMaterial = playerScript.gunList[playerScript.gunListPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

            playerScript.gunModel.GetComponent<MeshFilter>().sharedMesh = playerScript.gunList[0].gunModel.GetComponent<MeshFilter>().sharedMesh;
            playerScript.gunModel.GetComponent<MeshRenderer>().sharedMaterial = playerScript.gunList[0].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        }
    }

    public void ResetAllItems()
    {
        SaveItemStatus(bomb, enableBomb = false);
        SaveItemStatus(grenade, enableGrenade = false);
        SaveItemStatus(stunner, enableStunner = false);
        SaveItemStatus(pistol, hasPistol = false); 
        SaveItemStatus(smgun, hasPistol = false);
        SaveItemStatus(cannon, hasCannon = false);
        SaveItemStatus(flamethrower, hasPistol = false);
       

        if (InventoryManager.Instance != null)
        {
            
            InventoryManager.Instance.hotbarItems.Clear();
            for (int i = 0; i < InventoryManager.Instance.hotbarSize; i++)
            {
                InventoryManager.Instance.hotbarItems.Add(null);
            }
        }

    }

    public void updateKeysCollected(int amount)
    {
        keysCount += amount;
        keysCollectedText.text = keysCount.ToString("F0");
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
                //test
                //statePause();
                //menuActive = menuWin;
                //menuActive.SetActive(true);
                //SoundManager.Instance.PlayMusic(BGMusic, 0.2f);
                //SoundManager.Instance.PlayEffect(winMusic, 1);
                SoundManager.Instance.musicSource.loop = false;
                SoundManager.Instance.PlayMusic(winMusic, 1);
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
                //SoundManager.Instance.PlayEffect(winMusic, 1);
                SoundManager.Instance.musicSource.loop = false;
                SoundManager.Instance.PlayMusic(winMusic, 1);
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

    public int GetGameGoalCount()
    {
        return gameGoalCount;
    }
    public int GetKidsRescued()
    {
        return kidsRescued;
    }

    //Alien Spaceship final room 
   public void UpdateSoldiersKilled(int amount)
    {
        soldiersToKill += amount;

        if (soldiersToKill == 0)
        {
            if (finalDoor != null)
            {
                finalDoor.SetActive(false);
            }
        }
    }

    public void LaunchpadBossKilled()
    {
        launchpadBossKilled = true; 
    }

    //Credits screen
    public void EnableAssets()
    {
        assetsPopup.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton);
    }
    public void DisableAssets()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedCredits);
        assetsPopup.SetActive(false);
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

        //Don't need this because made winning sound effect into music
        //SoundManager.Instance.LowerVolumeInstantly();
        //PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);

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
        //SoundManager.Instance.LowerVolumeInstantly();
        //PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
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

    public IEnumerator FlashLaunchpadUI()
    {
        launchpadPopup.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        launchpadPopup.SetActive(false);

    }

    public IEnumerator WaitForKidsToSpawn()
    {
        if (!kidsSpawned)
            yield return new WaitForSeconds(2.0f);
        else
            SoundManager.Instance.PlayEffect(thankYou, 1);
        kidsSpawned = true;
    }

    public void SetAmmoIcon(Sprite _newSprite)
    {
        ammoIcon.sprite = _newSprite;
        ammoIcon.SetNativeSize();
    }
}
