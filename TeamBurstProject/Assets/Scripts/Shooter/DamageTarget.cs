using UnityEngine;

//Sample universal 

public class DamageTarget : MonoBehaviour, IDamage
{

    [Header("Health Settings")]
    [Tooltip("How much health this object starts with.")]
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("If checked, the GameObject is destroyed when health hits 0.")]
    [SerializeField] private bool destroyOnDeath = true;

    // We store the current health at runtime. "_ denotes an internal attribute typically not accessed directly"
    private float _health;

    //This initializes our current Health on creation (Awake happens before the first frame in Unity)
    private void Awake()
    {
        // Make sure we never start at 0 by accident (which would be "dead").
        _health = Mathf.Max(1f, maxHealth);
    }

    public bool IsAlive => _health > 0f; //This allows other functions to ask if the target is alive. Useful for objects that are damageable but we do not want to destroy if they have 0 HP.
    
    public void TakeDamage(int amount)
    {
        if (!IsAlive) return; //ignore further hits if we're already dead

        // Subtract damage; clamp so it never goes below 0.
        float dmg = Mathf.Max(0f, amount);
        _health = Mathf.Max(0f, _health - dmg);

        // Simple console feedback so you can SEE it's working while testing.
        Debug.Log($"{name} took {dmg} damage. HP: {_health}/{maxHealth}");

        // If health reached 0, we "die".
        if (_health <= 0f)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Debug.Log($"{name} died.");

        if (destroyOnDeath)
        {
            // Destroy the whole GameObject shortly after death.
            Destroy(gameObject, 0.05f);
        }
        else
        {
            // disable colliders / AI so it's "dead" but not destroyed.
            // foreach (var col in GetComponentsInChildren<Collider>()) col.enabled = false;
            // enabled = false; // disable this component
            // Typically for interactables that have HP or destructible environment objects that have a destructed state as well as players where the game ends when they die as opposed to being destroyed
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return; // no healing if already dead (change if we add revive)
        float heal = Mathf.Max(0f, amount);
        _health = Mathf.Min(maxHealth, _health + heal);
        Debug.Log($"{name} healed {heal}. HP: {_health}/{maxHealth}");
    }

    // (Optional) Expose read-only values for UI bars later.
    public float CurrentHealth => _health;
    public float MaxHealth => maxHealth;
}
