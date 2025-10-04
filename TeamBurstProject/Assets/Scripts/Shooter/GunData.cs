using UnityEngine;

//Creates a Unity Menu to create a new Gun Data file
[CreateAssetMenu(
    fileName = "NewGun",
    menuName = "Guns/Gun Data",
    order = 0)]
public class GunData : ScriptableObject, iGun
{
    //This will contain all information needed for each type of gun. Changes here will be made to ALL users of the gun (player and enemy)

    [Header("Identity")]
    [Tooltip("Unique id used for reference.")]
    [SerializeField] private string id = "pistol";

    [Header("Damage & Range")]
    [Tooltip("Base damage per shot.")]
    [SerializeField, Min(0f)] private int damage = 20;

    [Tooltip("Effective range in meters. Used by hitscan and as max travel for projectiles.")]
    [SerializeField, Min(0f)] private float range = 50f;

    [Header("Firing")]
    [Tooltip("Shots per second. Example: 5 = one shot every 0.2s.")]
    [SerializeField, Min(0.01f)] private float fireRate = 5f;

    [Tooltip("Hold to fire continuously if true; tap-to-fire if false.")]
    [SerializeField] private bool isAutomatic = false;

    [Tooltip("Random cone spread in degrees applied per shot (0 = perfectly accurate).")]
    [SerializeField, Range(0f, 15f)] private float spreadDegrees = 1.5f;

    [Header("Ammo & Reload")]
    [Tooltip("Rounds per magazine.")]
    [SerializeField, Min(1)] private int magazineSize = 12;

    [Tooltip("Seconds to reload from empty to full.")]
    [SerializeField, Min(0f)] private float reloadTime = 1.6f;

    [Header("Delivery")]
    [Tooltip("If true, uses a raycast (instant hit). If false, spawns a projectile prefab.")]
    [SerializeField] private bool isHitscan = true;

    [Tooltip("Projectile prefab used when IsHitscan is false (e.g., physical bullet).")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("Initial projectile speed in meters/second (used if IsHitscan == false).")]
    [SerializeField, Min(0f)] private float muzzleVelocity = 120f;

    // ===== iGun interface backing properties (read-only outside) =====
    public string Id => id;
    public int Damage => damage;
    public float Range => range;
    public float FireRate => fireRate;
    public bool IsAutomatic => isAutomatic;
    public float SpreadDegrees => spreadDegrees;
    public int MagazineSize => magazineSize;
    public float ReloadTime => reloadTime;
    public bool IsHitscan => isHitscan;
    public GameObject ProjectilePrefab => projectilePrefab;
    public float MuzzleVelocity => muzzleVelocity;

#if UNITY_EDITOR
    // This runs in the editor when values change—helps catch mistakes early.
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = name.ToLower().Replace(" ", "_");

        if (isHitscan)
        {
            // For hitscan, projectile settings are irrelevant.
            muzzleVelocity = Mathf.Max(0f, muzzleVelocity);
            // It's okay if projectilePrefab is null when hitscan.
        }
        else
        {
            // For projectile guns, remind ourselves to assign a prefab later.
            if (projectilePrefab == null)
            {
                // Just a gentle reminder in the Console while editing.
                // Debug.LogWarning($"GunData '{name}' is projectile-based but has no ProjectilePrefab assigned.");
            }
        }
    }
#endif

}
