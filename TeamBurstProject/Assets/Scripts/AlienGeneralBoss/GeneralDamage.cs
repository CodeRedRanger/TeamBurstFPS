using UnityEngine;

public class GeneralDamage : MonoBehaviour, IDamage
{
    [SerializeField] GameObject finalGoal;
    [SerializeField] int health;
    public void Heal(int amount)
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            finalGoal.SetActive(true);
            GameObject.Destroy(gameObject);
        }
    }
}
