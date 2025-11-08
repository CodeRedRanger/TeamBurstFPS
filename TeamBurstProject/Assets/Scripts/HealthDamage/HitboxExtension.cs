using UnityEngine;

public class HitboxExtension : MonoBehaviour, IDamage
{
    [SerializeField] Destructible primaryDamageScript;

    public void Heal(int amount)
    {
        primaryDamageScript.Heal(amount);
    }

    public void TakeDamage(int amount)
    {
        primaryDamageScript.TakeDamage(amount);
    }
}
