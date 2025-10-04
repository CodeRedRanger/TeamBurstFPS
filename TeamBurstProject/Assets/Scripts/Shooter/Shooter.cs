using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooter : MonoBehaviour
{
    [Header("Player View / Aim")]
    [Tooltip("Camera used to aim the gun (usually your Player camera).")]
    [SerializeField] private Camera viewCamera;

    [Header("Equipped Gun")]
    [Tooltip("Drop a GunData asset here (e.g., Pistol, Rifle).")]
    [SerializeField] private GunData equippedGun; // implements iGun

    [Header("Layers")]
    [Tooltip("Which layers the hitscan ray should test against (default: Everything).")]
    [SerializeField] private LayerMask hitMask = ~0; // Everything

    [Header("Debug")]
    [Tooltip("Draw a short debug ray in the Scene view each time you fire.")]
    [SerializeField] private bool drawDebugRay = true;

    // --- Internal runtime state ---
    private int _ammoInMag;
    private bool _isReloading;
    private float _cooldownTimer; // counts down; can fire when 0
    private float _reloadTimer; // Counts down; 

    //At the very start of the game if this has not been assigned to the Player's camera try to do that.
    private void Awake()
    {
        if (viewCamera == null && gameObject.CompareTag("Player")) viewCamera = Camera.main;
        _ammoInMag = (equippedGun != null) ? equippedGun.MagazineSize : 0;
    }

    // NOTES: For AI, you can call CanFire/FireOnce from your AI code instead.
    private void Update()
    {
        if (!gameObject.CompareTag("Player")) return;
        if (equippedGun == null || viewCamera == null) return;

        // 1) Tick fire-rate cooldown toward 0
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;

        // 2) Handle reload timer if we're reloading
       TickReloadTimer();

        // 3) Player input: fire
        if (Input.GetButton("Fire1"))
        {
            TryFire();
        }

        // 4) Player input: manual reload (R key)
        if (Input.GetButton("Reload"))
        {
            StartReload(equippedGun);
        }
    }

    private void TryFire()
    {
        if (!CanFire(equippedGun)) return;

        // Build a direction from the camera, with random spread applied.
        Vector3 origin = viewCamera.transform.position;
        Vector3 forward = viewCamera.transform.forward;
        Vector3 shotDir = ApplySpread(forward, equippedGun.SpreadDegrees);

        FireOnce(equippedGun, origin, shotDir);
    }

    public void FireOnce(iGun gun, Vector3 origin, Vector3 direction)
    {
        // 1) Consume one round and start the fire-rate cooldown.
        _ammoInMag = Mathf.Max(0, _ammoInMag - 1);
        _cooldownTimer = 1f / Mathf.Max(0.01f, gun.FireRate);

        // Optional debug ray in Scene view for visibility.
        if (drawDebugRay)
            Debug.DrawRay(origin, direction * gun.Range, Color.white, 0.1f);

        // HITSCAN: Raycast forward to see what we hit.
        if (Physics.Raycast(origin, direction, out RaycastHit hit, gun.Range, hitMask, QueryTriggerInteraction.Ignore))
        {
            // Try to find something damageable on what we hit (or its parents).
            var damage = hit.collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamage(gun.Damage);
            }

            // TODO (optional): spawn impact VFX or decals at 'hit.point'
            // Instantiate(impactVfx, hit.point, Quaternion.LookRotation(hit.normal));
        }

        // 4) Auto-reload when empty (optional but friendly).
        if (_ammoInMag <= 0)
        {
            StartReload(gun);
        }
    }

    public void StartReload(iGun gun)
    {
        if (gun == null) return;
        if (_isReloading) return;
        if (_ammoInMag >= gun.MagazineSize) return; // already full

        _isReloading = true;
        _reloadTimer = gun.ReloadTime;
    }

    private void TickReloadTimer()
    {
        if (!_isReloading) return;

        _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0f)
        {
            _isReloading = false;
            _ammoInMag = equippedGun != null ? equippedGun.MagazineSize : _ammoInMag;
            // TODO (optional): play "reload finished" SFX/animation here.
        }
    }

    //Quick Check to See if the gun is currently in a position to fire.
    public bool CanFire(iGun gun)
    {
        if (gun == null) return false;
        if (_isReloading) return false;
        if (_cooldownTimer > 0f) return false;
        if (_ammoInMag <= 0) return false;
        return true;
    }
    
    //Bullet Recoil / Shotgun spread / Bullet Deviation
    private Vector3 ApplySpread(Vector3 forward, float degrees)
    {
        if (degrees <= 0.0001f) return forward; // If we have no recoil -> Exit (The Bullet fires directly ahead)

        // Generate bullet spread by getting a random value within the gun's recoil angle
        float yaw = Random.Range(-degrees * 0.5f, degrees * 0.5f); //Yaw refers to turning your head side to side (Like saying no)
        float pitch = Random.Range(-degrees * 0.5f, degrees * 0.5f); //Pitch refers to turning your head up and down (Like Nodding)

        //Apply the changes we created
        Vector3 dir = Quaternion.AngleAxis(yaw, Vector3.up) * forward;
        dir = Quaternion.AngleAxis(pitch, viewCamera.transform.right) * dir;
        
        //Return those changes
        return dir.normalized;
    }
}
