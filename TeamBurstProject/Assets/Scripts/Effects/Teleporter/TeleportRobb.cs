using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    // This is where our teleport will send the player.
    // Place an empty GameObject at the point in the world you want to send the player to.
    [SerializeField] Transform teleportPos;
    

    
    private int currentgoalCount;
    private int requiredGoalCount = 0; 


    void Start()
    {
        currentgoalCount = gameManager.instance.GetGameGoalCount();
    }

    void Update()
    {

        currentgoalCount = gameManager.instance.GetGameGoalCount();
    }


    void OnTriggerEnter(Collider other)
    {

        currentgoalCount = gameManager.instance.GetGameGoalCount();

        if (other.CompareTag("Player"))
        {

            // When a player enters this trigger, set their position to the teleport position.
            if (currentgoalCount <= requiredGoalCount)
            {
                Teleport();
            }
        }
    }

    

    public void Teleport()
    {
        // Move the player to the teleport position.
        CharacterController cc = gameManager.instance.player.GetComponent<CharacterController>();

        if (cc != null)
       {
          cc.enabled = false;
         
          if (gameManager.instance.player != null && teleportPos != null)
           {
               gameManager.instance.player.transform.position = teleportPos.position;
               Debug.Log("Transported!");
           }
           else if (gameManager.instance.player == null)
           {
               Debug.LogWarning("Player reference is null!");
           }
           else if (teleportPos == null)
           {
               Debug.LogWarning("Teleport position reference is null!");
           }
           cc.enabled = true;
        }
        else
        {
        // If no CharacterController is found, just set the position directly.
           gameManager.instance.player.transform.position = teleportPos.position;
         }


    }

}
