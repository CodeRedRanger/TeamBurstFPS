using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    // This is where our teleport will send the player.
    // Place an empty GameObject at the point in the world you want to send the player to.
    [SerializeField] Transform teleportPos;

    private void OnTriggerEnter(Collider other)
    {
        int currentgoalCount = gameManager.instance.GetGameGoalCount();

        // When a player enters this trigger, set their position to the teleport position.
        if (other.CompareTag("Player") && currentgoalCount < 3)
        {

            gameManager.instance.player.transform.position = teleportPos.position;

        }

    }

}
