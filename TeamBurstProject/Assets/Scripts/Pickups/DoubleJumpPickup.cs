using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DoubleJumpPickup : MonoBehaviour, IPickup
{
    [SerializeField] AudioClip pickupSound;
    [SerializeField] int value;
    [SerializeField] float length;
    [SerializeField] bool destroyOnPickup;
    [SerializeField] bool isPermanent;

    private bool hasBeenPickedUp = false;

    public void Pickup()
    {

        //Debug.Log("Picked up boost");
        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            SoundManager.Instance.PlayEffect(pickupSound, 1);

            // Hide visuals and collider
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;


            StartCoroutine(DoBoost());
        }

    }

    IEnumerator DoBoost()
    {
        if (isPermanent)
        {
            gameManager.instance.playerScript.SetJumpCountMax(value);
            StartCoroutine(doublejumpFeedback());
        }
        else
        {
            int prev = gameManager.instance.playerScript.GetJumpCountMax();
            gameManager.instance.playerScript.SetJumpCountMax(value);
            StartCoroutine(doublejumpFeedback());
            yield return new WaitForSeconds(length);
            gameManager.instance.playerScript.SetJumpCountMax(prev);
        }


        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            hasBeenPickedUp = false;
        }
    }

    IEnumerator doublejumpFeedback()
    {
        gameManager.instance.doublejumpPopup.SetActive(true);
        yield return new WaitForSeconds(length);
        gameManager.instance.doublejumpPopup.SetActive(false);
    }

}






