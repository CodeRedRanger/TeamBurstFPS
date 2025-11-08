using UnityEngine;

public class GeneralDamage : MonoBehaviour, IDamage
{
    [SerializeField] int health;
    public void Heal(int amount)
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
            GameObject.Destroy(gameObject);
    }
}
