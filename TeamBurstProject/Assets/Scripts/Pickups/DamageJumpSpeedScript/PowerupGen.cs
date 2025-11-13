using System.Collections;
using UnityEngine;

public class PowerupGen : MonoBehaviour
{
    // Uses an enum to decide which powerup it is.
    enum PowerupType { Speed, Damage, Jump, Invincible }

    [SerializeField] PowerupType type;

    [SerializeField] int boostAmount;
    [SerializeField] float length;
    [SerializeField] bool destroyOnPickup;
    [SerializeField] AudioClip pickupSound;

    bool isPickedUp;
    private int speedOrig; 

    // Update is called once per frame
    void Start()
    {
       speedOrig = gameManager.instance.playerScript.speed;
    }

private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        // Hide powerup until it is destroyed or reappears.
        if(!isPickedUp)
        {        
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            isPickedUp = true;

            // Begin coroutine method.
            SoundManager.Instance.PlayEffect(pickupSound, 1);
            StartCoroutine(AddPowerup());
        }

    }

    IEnumerator AddPowerup()
    {
        // Change different stats depending on powerup type.
        switch (type)
        {
            case PowerupType.Speed:
                gameManager.instance.playerScript.SpeedBoost(boostAmount);
                PopupManager.instance.ShowPopup("speed boost", length);
                break;

            case PowerupType.Damage:
                gameManager.instance.playerScript.AddShootDamage(boostAmount);
                break;

            case PowerupType.Jump:
                gameManager.instance.playerScript.AddJumpSpeed(boostAmount);
                break;
            case PowerupType.Invincible:
                gameManager.instance.playerScript.invinciblePowerupActive = true;
                gameManager.instance.playerScript.SetInvincibility(true);
                PopupManager.instance.ShowPopup("invincibility", length);
                break;
        }

        // Wait for powerup to run out.
        yield return new WaitForSeconds(length);

        // Change stats back to normal.
        switch (type)
        {
            case PowerupType.Speed:
                gameManager.instance.playerScript.SpeedBoost(-boostAmount);
                if(gameManager.instance.playerScript.speed < speedOrig)
                {
                    gameManager.instance.playerScript.speed = speedOrig;
                }

                break;

            case PowerupType.Damage:
                gameManager.instance.playerScript.AddShootDamage(-boostAmount);
                break;

            case PowerupType.Jump:
                gameManager.instance.playerScript.AddJumpSpeed(-boostAmount);
                break;
            case PowerupType.Invincible:
                gameManager.instance.playerScript.invinciblePowerupActive = false;
                gameManager.instance.playerScript.SetInvincibility(false);
                break;
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
            isPickedUp = false;
        }

    }

    // DELETE LATER
    IEnumerator speedboostFeedback()
    {
        gameManager.instance.speedboostPopup.SetActive(true);
        yield return new WaitForSeconds(length);
        gameManager.instance.speedboostPopup.SetActive(false);
    }

    // DELETE LATER
    IEnumerator invincibleFeedback()
    {
        gameManager.instance.invinciblePopup.SetActive(true);
        yield return new WaitForSeconds(length);
        gameManager.instance.invinciblePopup.SetActive(false);
    }

}
