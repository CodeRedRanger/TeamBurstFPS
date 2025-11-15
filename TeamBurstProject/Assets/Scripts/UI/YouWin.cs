using UnityEngine;
using System.Collections; 



public class YouWin : MonoBehaviour
{

    [SerializeField] AudioClip youWin;
    [SerializeField] bool gameOver;
    bool triggered = false;
    int launchpadLevel = 5; 
    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        if (other.CompareTag("Player") && triggered == false)
        {
            if (gameManager.instance.currentScene.buildIndex == launchpadLevel && (gameManager.instance.keysRequired > gameManager.instance.keysCount 
                || gameManager.instance.launchpadBossKilled == false))
            {
                return; 
            }

            triggered = true;
           
            //SoundManager.Instance.PlayEffect(youWin, 1);
            SoundManager.Instance.musicSource.loop = false;
            SoundManager.Instance.PlayMusic(youWin, 1);
            //SoundManager.Instance.StopMusic();
            

            if (gameOver == false)
            {
                gameManager.instance.youWin();
                //implemented in first flagpole script
                //gameManager.instance.Level1 = false;
            }
            else
            {
                gameManager.instance.youWinEnd(); 
            }
            

        }
    }

   



   }
