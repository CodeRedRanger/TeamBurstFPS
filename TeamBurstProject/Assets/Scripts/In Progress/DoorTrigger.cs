using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] DoorController door;
    [SerializeField] float openAngle = 90;
    [SerializeField] float triggerDistance;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= triggerDistance)
        {
            door.OpenDoor(openAngle);
        }
    }

    void Awake()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }
    }
}
