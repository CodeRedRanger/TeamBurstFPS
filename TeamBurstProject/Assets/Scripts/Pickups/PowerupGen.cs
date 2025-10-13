using UnityEngine;

public class PowerupGen : MonoBehaviour
{
    // Uses an enum to decide which powerup it is.
    enum PowerupType { Speed, Damage, Jump }

    [SerializeField] PowerupType type;

    [SerializeField] int boostAmount;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        // Change different stats depending on powerup type.
        if (type == PowerupType.Speed)
        {
            gameManager.instance.playerScript.SpeedBoost(boostAmount);
        }
        if (type == PowerupType.Damage)
        { 
            gameManager.instance.playerScript.AddShootDamage(boostAmount);
        }
        if (type == PowerupType.Jump)
        { 
            gameManager.instance.playerScript.AddJumpSpeed(boostAmount);
        }

        Destroy(gameObject);

    }

}
