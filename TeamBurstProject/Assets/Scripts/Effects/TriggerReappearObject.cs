using System.Collections;
using UnityEngine;


public class TriggerReappearObject : MonoBehaviour
{
    public GameObject objectToAppear;
    public float delay = 1f; // Delay in seconds before the object appears
    //[SerializeField] string tagToTrigger; 
    [SerializeField] bool bossFight;
    //bool voicePlayed = false;
    //[SerializeField] AudioClip forBoss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

   

        if (other.CompareTag("Player"))
        {
            if (objectToAppear != null)
            {
                if(objectToAppear.activeSelf == false)
                    objectToAppear.SetActive(true); 
                //StartCoroutine(ShowObjectAfterDelay(delay)); // Start the coroutine to show the object after a delay
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
    
