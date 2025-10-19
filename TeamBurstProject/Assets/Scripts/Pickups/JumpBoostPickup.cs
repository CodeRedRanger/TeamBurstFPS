using System.Collections;
using UnityEngine;

public class JumpBoostPickup : MonoBehaviour, IPickup
{
    [SerializeField] AudioClip pickupSound;
    [SerializeField] int value;
    [SerializeField] float length;
    [SerializeField] bool destroyOnPickup;
   

    private bool hasBeenPickedUp = false;

    public void Pickup()
    {

        Debug.Log("Picked up boost"); 
        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            SoundManager.Instance.PlayEffect(pickupSound, 1);

            // Hide visuals and collider
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            
            StartCoroutine(AddJump());
        }

    }

    IEnumerator AddJump()
    {
        gameManager.instance.playerScript.AddJumpSpeed(value);
        StartCoroutine(jumpboostFeedback());
        yield return new WaitForSeconds(length);
        gameManager.instance.playerScript.AddJumpSpeed(-value);

       if (destroyOnPickup)
        {
            Debug.Log("Getting Here"); 
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            hasBeenPickedUp = false; 
        }
    }

    IEnumerator jumpboostFeedback()
    {
        gameManager.instance.jumpboostPopup.SetActive(true);
        yield return new WaitForSeconds(length);
        gameManager.instance.jumpboostPopup.SetActive(false);
    }
}

