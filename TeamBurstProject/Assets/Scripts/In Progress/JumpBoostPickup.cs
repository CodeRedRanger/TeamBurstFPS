using System.Collections;
using UnityEngine;

public class JumpBoostPickup : MonoBehaviour, IPickup
{
    [SerializeField] AudioClip pickupSound;
    [SerializeField] int value;
    [SerializeField] float length;

    private bool hasBeenPickedUp = false;

    public void Pickup()
    {
        Debug.Log("Getting here");

        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            SoundManager.Instance.PlayEffect(pickupSound);

            // Hide visuals and collider
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            
            StartCoroutine(AddJump());
        }

    }

    IEnumerator AddJump()
    {
        gameManager.instance.playerScript.AddJumpSpeed(value);
        yield return new WaitForSeconds(length);
        gameManager.instance.playerScript.AddJumpSpeed(-value);
        Destroy(gameObject);
    }
}

