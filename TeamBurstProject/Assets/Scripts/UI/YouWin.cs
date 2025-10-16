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
            triggered = true;
            SoundManager.Instance.PlayEffect(youWin, 1);
            SoundManager.Instance.StopMusic();

            if (gameOver == false)
            {
                gameManager.instance.youWin();
                gameManager.instance.Level1 = false;
            }
            else
            {
                gameManager.instance.youWinEnd(); 
            }

            
        }
    }

   }
