using System.Collections;
using UnityEngine;

public class PowerupGen : MonoBehaviour
{
    // Uses an enum to decide which powerup it is.
    enum PowerupType { Speed, Damage, Jump }

    [SerializeField] PowerupType type;

    [SerializeField] int boostAmount;
    [SerializeField] float length;
    [SerializeField] bool destroyOnPickup;
    [SerializeField] AudioClip pickupSound;

    bool isPickedUp;

    // Update is called once per frame
    void Update()
    {
        
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
            SoundManager.Instance.PlayEffect(pickupSound);
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
                break;

            case PowerupType.Damage:
                gameManager.instance.playerScript.AddShootDamage(boostAmount);
                break;

            case PowerupType.Jump:
                gameManager.instance.playerScript.AddJumpSpeed(boostAmount);
                break;
        }

        // Wait for powerup to run out.
        yield return new WaitForSeconds(length);

        // Change stats back to normal.
        switch (type)
        {
            case PowerupType.Speed:
                gameManager.instance.playerScript.SpeedBoost(-boostAmount);
                break;

            case PowerupType.Damage:
                gameManager.instance.playerScript.AddShootDamage(-boostAmount);
                break;

            case PowerupType.Jump:
                gameManager.instance.playerScript.AddJumpSpeed(-boostAmount);
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

}
