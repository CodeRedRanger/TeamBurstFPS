using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Lifetime")]
    [Tooltip("How many seconds this bullet exists before auto-destroying.")]
    [SerializeField] private float lifetime = 5f;

    [Header("Physics (optional)")]
    [Tooltip("If you want gravity, set this > 0 (meters/second^2). 0 = no gravity.")]
    [SerializeField] private float gravity = 0f;

    // --- Runtime fields set by Init(...) ---
    private float _damage;
    private GameObject _owner;
    private Vector3 _velocity;   // meters/second
    private bool _initialized;

    //Called by the Shooter right after the bullet is spawned to pass data into the projectile. (Initialize the bullet)
    public void Init(float damage, Vector3 direction, GameObject owner, float muzzleVelocity)
    {
        _damage = damage;
        _owner = owner;

        // Face the movement direction and set our initial velocity.
        direction = direction.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        _velocity = direction * Mathf.Max(0f, muzzleVelocity);

        _initialized = true;
    }
}
