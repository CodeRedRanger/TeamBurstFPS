using UnityEngine;

public class YouWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        if (other.CompareTag("Player"))
        {
            gameManager.instance.youWin();
        }
    }

   }
