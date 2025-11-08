using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public enum GrappleState
    {
        Idle, //Not doing anything related to grapple
        Aiming, // Looking for a grapple Point. (Raycast)
        Firing, //Stretch Goal State for firing a visible projectile
        Attached, // Rope is attached -> Logic for pull will be here
        Cooldown //Grapple lockout state
    }
}
