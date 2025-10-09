using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class gameManager : MonoBehaviour


{
    public static gameManager instance;
    //any open menu will go into menuActive and then close active menu 
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        //need this line before next
       player = GameObject.FindGameObjectWithTag("Player");
       playerScript = player.GetComponent<PlayerController>();

       SoundManager.Instance.PlayMusic(BGMusic);

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
            SoundManager.Instance.PlayEffect(toSchool);



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
