using System.Collections;
using UnityEngine;

public class TriggerAppearObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject objectToAppear; 
    public float delay = 1f; // Delay in seconds before the object appears
    //[SerializeField] string tagToTrigger; 
    [SerializeField] bool bossFight;
  

    [SerializeField] AudioClip forBoss; 

    void Start()
    {
        if (objectToAppear != null)
        {
            objectToAppear.SetActive(false); // Ensure the object is initially inactive
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Rocket") || other.CompareTag("Player")) // Check if the colliding object has the "Rocket" or Player tag
        //can make a tag variable if want to use for other things
        {
            if (objectToAppear != null)
            {
                StartCoroutine(ShowObjectAfterDelay(delay)); // Start the coroutine to show the object after a delay
            }
        }

        if (bossFight == true && other.CompareTag("Player"))
        {
            if (forBoss != null)
            {
                SoundManager.Instance.PlayEffect(forBoss);
     
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

