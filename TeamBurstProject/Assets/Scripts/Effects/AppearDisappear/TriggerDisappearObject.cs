using UnityEngine;
using System.Collections;

public class TriggerDisappearObject : MonoBehaviour
{
    

    [SerializeField] GameObject objectToDisappear;
    public float delay = 1f; // Delay in seconds before the object appears
    private MeshRenderer meshRenderer; 

    //Options for this script
    //[SerializeField] string tagToTrigger; 
    //[SerializeField] bool bossFight;
    //bool voicePlayed = false;


    //[SerializeField] AudioClip forBoss;

    void Start()
    {
        if (objectToDisappear != null)
        {
            //if mesh
            meshRenderer = objectToDisappear.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;// Ensure the mesh is initially active
           //if object
           //objectToAppear.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) // Check if the colliding object has the "Rocket" or Player tag
        //can make a tag variable if want to use for other things
        {
            if (objectToDisappear != null)
            {
                //if mesh
                meshRenderer.enabled = false; 

                //if object
                //objectToAppear.SetActive(false);

                //optional if want a delay
                //StartCoroutine(RemoveObjectAfterDelay(delay)); // Start the coroutine to show the object after a delay
            }
        }

        //Use template below to make sound effects occur with disappearance
        /*
        if (bossFight == true && other.CompareTag("Player") && voicePlayed == false)
        {
            if (forBoss != null)
            {
                SoundManager.Instance.PlayEffect(forBoss);
                voicePlayed = true;


            }
        }*/

    }

    /*
    IEnumerator RemoveObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (objectToDisappear != null)
        {
            //if object
            objectToDisappear.SetActive(false); // Activate the object after the delay
        }
    }*/
}

