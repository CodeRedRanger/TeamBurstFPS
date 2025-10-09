using UnityEngine;

public interface iShooter
{
    // Interface for anything that can fire a gun (player, enemy, turret).
    //Player and enemies share the same shooting contract. This ensures consistency across the board.

    // Quick check: do we have ammo, not reloading, fire-rate ready, etc.?
    bool CanFire(iGun gun);

    // Fire once right now. The shooter decides hitscan vs projectile using the gun data.
    // origin: where the shot leaves (muzzle/camera)
    // direction: where we aim
    void FireOnce(iGun gun, Vector3 origin, Vector3 direction);

    // Called when holding down fire with automatic weapons.
    // We'll wire continuous fire in a later step.
    void BeginHoldFire(iGun gun);
    void EndHoldFire(iGun gun);

    // Reload request (begins a reload if possible).
    void StartReload(iGun gun);
}
