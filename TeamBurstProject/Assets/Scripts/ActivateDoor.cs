using UnityEngine;

public class ActivateDoor : MonoBehaviour
{
    private int currentgoalCount;
    private int requiredGoalCount = 0;
    public GameObject doorToActivate; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentgoalCount = gameManager.instance.GetGameGoalCount();
    }

    // Update is called once per frame
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
                
                doorToActivate.SetActive(true);
            }
        }
    }


}

