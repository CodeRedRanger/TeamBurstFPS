using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour, IDamage
{
    [SerializeField] int maxHP;
    int currentHP;
    [SerializeField] UnityEvent destroyedEvent;
    [SerializeField] UnityEvent takeDamageEvent;
    [SerializeField] GameObject objectToDestroy;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        takeDamageEvent.Invoke();
        if(currentHP <= 0) destroyedEvent.Invoke();
    }



    // Damage Events

    public void SimpleDamageFlash()
    {

    }



    // Destroyed Events

    public void SimpleDestroy()
    {
        Destroy(objectToDestroy);
    }
}
