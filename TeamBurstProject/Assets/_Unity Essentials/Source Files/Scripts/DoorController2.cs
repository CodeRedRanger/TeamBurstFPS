using UnityEngine;

public class DoorController2 : MonoBehaviour
{
    private Animator doorAnimator;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Open the door");
        doorAnimator.SetTrigger("Door_Open");
        
    }
}
