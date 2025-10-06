using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController doorHinge;
    public float customOpenAngle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            doorHinge.OpenDoor(customOpenAngle);
        }
        
    }
}
