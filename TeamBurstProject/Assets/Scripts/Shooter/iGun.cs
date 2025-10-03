using UnityEngine;

public interface iGun
{
    // Interface for GUN DATA (no logic). A ScriptableObject will implement this.
    //Any “gun data” asset will implement this, so the shooter can read the same fields no matter the weapon.

    // Unique id for reference.
    string Id { get; }

    // Core ballistics / damage.
    float Damage { get; }          // base damage per shot
    float Range { get; }           // effective range in meters for hitscan; max travel for projectiles (destroy with distance instead of time)

    // Firing behavior.
    float FireRate { get; }        // shots per second (e.g., 10 = very fast)
    bool IsAutomatic { get; }      // true = hold to fire repeatedly
    float SpreadDegrees { get; }   // random cone spread per shot 

    // Ammo & reload.
    int MagazineSize { get; }      // rounds per mag
    float ReloadTime { get; }      // seconds to reload

    // Delivery: hitscan or projectile.
    bool IsHitscan { get; }        // true = ray-based/instant, false = projectile bullet
    GameObject ProjectilePrefab { get; } // used if IsHitscan == false
    float MuzzleVelocity { get; }  // m/s for projectile bullets


}
