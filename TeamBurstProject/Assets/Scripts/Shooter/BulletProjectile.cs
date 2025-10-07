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
}
