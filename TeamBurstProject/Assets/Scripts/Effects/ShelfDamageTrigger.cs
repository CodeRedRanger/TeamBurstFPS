using UnityEngine;

public class ShelfDamageTrigger : MonoBehaviour
{
    [SerializeField] int damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        IDamage player = other.GetComponent<IDamage>();

        if (player == null)
            return;

        player.TakeDamage(damageAmount);
    }
}
