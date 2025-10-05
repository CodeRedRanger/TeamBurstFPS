using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    // This is where our teleport will send the player.
    // Place an empty GameObject at the point in the world you want to send the player to.
    [SerializeField] Transform teleportPos;

    private void OnTriggerEnter(Collider other)
    {
        
        // When a player enters this trigger, set their position to the teleport position.
        if (other.CompareTag("Player"))
        {

            gameManager.instance.player.transform.position = teleportPos.position;

        }

    }

}
