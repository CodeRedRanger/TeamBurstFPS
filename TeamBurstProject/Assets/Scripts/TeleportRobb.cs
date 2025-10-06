using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    // This is where our teleport will send the player.
    // Place an empty GameObject at the point in the world you want to send the player to.
    [SerializeField] Transform teleportPos;

    private bool canTeleport = false;
    private int currentgoalCount; 

    void Start()
    {
        currentgoalCount = gameManager.instance.GetGameGoalCount();
    }

    void Update()
    {
        if(canTeleport)
        {
            gameManager.instance.player.transform.position = teleportPos.position;
            canTeleport = false;
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        currentgoalCount = gameManager.instance.GetGameGoalCount();

        // When a player enters this trigger, set their position to the teleport position.
        if (other.CompareTag("Player") && currentgoalCount < 3)
        {

            canTeleport = true;

        }

    }

}
