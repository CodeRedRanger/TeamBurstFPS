using UnityEngine;

public class Pit : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController playerController = gameManager.instance.player.GetComponent<PlayerController>();
            playerController.instantDeath();
        }
    }
}
