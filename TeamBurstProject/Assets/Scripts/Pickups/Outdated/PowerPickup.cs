using UnityEngine;

public class PowerPickup : MonoBehaviour
{

    // TODO: Add visual and simple bobbing up and down animation


    [SerializeField] int damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        gameManager.instance.playerScript.AddShootDamage(damageAmount);

        Destroy(gameObject);
    }
}
