using UnityEngine;



public class YouWin : MonoBehaviour
{

    [SerializeField] AudioClip youWin;
    [SerializeField] bool gameOver;
    bool triggered = false; 
    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        if (other.CompareTag("Player") && triggered == false)
        {
            if (gameManager.instance.currentScene.buildIndex == 4 && gameManager.instance.keysRequired > gameManager.instance.keysCount)
            {
                return; 
            }

            triggered = true;
            SoundManager.Instance.PlayEffect(youWin, 1);
            SoundManager.Instance.StopMusic();

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
