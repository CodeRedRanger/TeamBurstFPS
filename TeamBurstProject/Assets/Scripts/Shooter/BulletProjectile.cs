using UnityEngine;

public class BulletProjectile : MonoBehaviour, iProjectile
{
    [Header("Lifetime")]
    [Tooltip("How many seconds this bullet exists before auto-destroying.")]
    [SerializeField] private float lifetime = 5f;

    [Header("Physics (optional)")]
    [Tooltip("If you want gravity, set this > 0 (meters/second^2). 0 = no gravity.")]
    [SerializeField] private float gravity = 0f;

    // --- Runtime fields set by Init(...) ---
    private int _damage;
    private GameObject _owner;
    private Vector3 _velocity;   // meters/second
    private bool _initialized;

    //Called by the Shooter right after the bullet is spawned to pass data into the projectile. (Initialize the bullet)
    public void Init(int damage, Vector3 direction, GameObject owner, float muzzleVelocity)
    {
        _damage = damage;
        _owner = owner;

        // Face the movement direction and set our initial velocity.
        direction = direction.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        _velocity = direction * Mathf.Max(0f, muzzleVelocity);

        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized) return;

        // Apply gravity if enabled (v = v + g * dt downward).
        if (gravity > 0f)
        {
            _velocity += Vector3.down * gravity * Time.deltaTime;
        }

        // Move by velocity * time.
        transform.position += _velocity * Time.deltaTime;

        // Reduce lifetime and destroy when time runs out. (We can turn time into a CoRoutine if neccesary for grade in class)
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions with the owner and anything inside owner's hierarchy.
        if (_owner != null && other.transform.IsChildOf(_owner.transform))
            return;

        // Try to find something that can take damage.
        var dmg = other.GetComponentInParent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(_damage);
        }

        // (Optional) You could spawn an impact VFX here.

        Destroy(gameObject);
    }

}
