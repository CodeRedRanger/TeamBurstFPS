using UnityEngine;

public interface iProjectile
{
    //interface to enable shooters to initialize any bullet regardless of implementation changes

    // Called immediately after Instantiate().
    // damage: damage on hit
    // direction: initial forward direction
    // owner: who fired (so we can ignore self-collisions)
    // muzzleVelocity: initial speed in meters/second
    void Init(float damage, Vector3 direction, GameObject owner, float muzzleVelocity);
}
