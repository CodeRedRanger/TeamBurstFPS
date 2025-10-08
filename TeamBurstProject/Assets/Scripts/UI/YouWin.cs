using UnityEngine;



public class YouWin : MonoBehaviour
{

    [SerializeField] AudioClip youWin; 
    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlayEffect(youWin);
            SoundManager.Instance.StopMusic(); 
            gameManager.instance.youWin();
        }
    }

   }
