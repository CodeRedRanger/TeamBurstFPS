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

    

}
