using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public enum GrappleState
    {
        Idle, //Not doing anything related to grapple
        Aiming, // Looking for a grapple Point. (Raycast)
        Firing, //Stretch Goal State for firing a visible projectile
        Attached, // Rope is attached -> Logic for pull will be here
        Lockout //Grapple lockout state
    }

    [SerializeField] private GrappleState currentState = GrappleState.Idle;
    private GrappleState previousState = GrappleState.Idle;

    [SerializeField] private float lockoutTimer = 0.5f; // How long we sit in Lockout State before returning to idle
    private float lockoutCount;

    //Temp Input System: To test functionality without creating or assigning keys. 
    [Header("Temporary Test Inputs")]
    [Tooltip("Hold to Aim, release to stop Aiming.")]
    [SerializeField] private KeyCode aimKey = KeyCode.Mouse1;   // Right mouse
    [Tooltip("Tap to Attach while Aiming (fake for now).")]
    [SerializeField] private KeyCode attachKey = KeyCode.Mouse0; // Left mouse
    [Tooltip("Tap to Detach while Attached.")]
    [SerializeField] private KeyCode detachKey = KeyCode.Space;  // Space bar

    void Update()
    {
        switch (currentState)
        {
            case GrappleState.Idle:
                HandleIdle();
                break;

            case GrappleState.Aiming:
                HandleAiming();
                break;

            case GrappleState.Firing:
                HandleFiring();
                break;

            case GrappleState.Attached:
                HandleAttached();
                break;

            case GrappleState.Lockout:
                HandleLockout();
                break;
        }
    }

    private void HandleIdle()
    {
        if (Input.GetKey(aimKey))
        {
            ChangeState(GrappleState.Aiming);
        }
    }

    private void HandleAiming()
    {
        // Stop aiming if the player lets go of the aim key
        if (!Input.GetKey(aimKey))
        {
            ChangeState(GrappleState.Idle);
            return;
        }

        // TEMP: pretend we found a valid point and attached when left-clicked
        if (Input.GetKeyDown(attachKey))
        {
            ChangeState(GrappleState.Attached);
        }
    }

    private void HandleFiring()
    {
        // For now, just snap to Attached to visualize flow. 
       // When adding a projectile: Enter Handle Firing -> Shoot Projectile -> Wait for projectile hit -> Proceed as we do now
        ChangeState(GrappleState.Attached);
    }

    private void HandleAttached()
    {
        //If a player presses Jump while attached they should detach and begin falling.
        if (Input.GetKeyDown(detachKey))
        {
            StartLockout();
        }
    }

    private void HandleLockout()
    {
        lockoutCount -= Time.deltaTime;
        if (lockoutCount <= 0f)
        {
            ChangeState(GrappleState.Idle);
        }
    }

    private void ChangeState(GrappleState next)
    {
        if (next == currentState) return;

        previousState = currentState;
        currentState = next;

        Debug.Log($"[Grapple] {previousState} -> {currentState}");
        OnEnterState(currentState);
    }

    private void OnEnterState(GrappleState state)
    {
        switch (state)
        {
            case GrappleState.Idle:
                // Reset any temporary aiming visuals
                break;

            case GrappleState.Aiming:
                // Show crosshair highlight
                break;

            case GrappleState.Firing:
                // Spawn projectile later.
                break;

            case GrappleState.Attached:
                // Create rope visuals.
                break;

            case GrappleState.Lockout:
                // Nothing special here.
                break;
        }
    }

    private void StartLockout()
    {
        lockoutCount = lockoutTimer;
        ChangeState(GrappleState.Lockout);
    }
}
