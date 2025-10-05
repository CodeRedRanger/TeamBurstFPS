using System.Collections;
using UnityEngine;

public class TriggerAppearObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject objectToAppear; 
    public float delay = 1f; // Delay in seconds before the object appears
    void Start()
    {
        if (objectToAppear != null)
        {
            objectToAppear.SetActive(false); // Ensure the object is initially inactive
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket")) // Check if the colliding object has the "Rocket" tag
        {
            if (objectToAppear != null)
            {
                StartCoroutine(ShowObjectAfterDelay(delay)); // Start the coroutine to show the object after a delay
            }
        }
    }

    IEnumerator ShowObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (objectToAppear != null)
        {
            objectToAppear.SetActive(true); // Activate the object after the delay
        }
    }
}

