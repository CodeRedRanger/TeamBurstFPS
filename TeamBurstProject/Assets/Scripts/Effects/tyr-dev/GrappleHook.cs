using UnityEngine;
using UnityEngine.UI;
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

    [Header("Lockout Timer")]
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

    [Header("Aiming / Raycast")]
    [Tooltip("Camera used to aim the grapple (usually the player's camera).")]
    [SerializeField] private Camera aimCamera;
    [Tooltip("Only these layers are valid grapple targets.")]
    [SerializeField] private LayerMask grappleLayers;
    [Tooltip("Maximum distance the grapple can reach.")]
    [SerializeField] private float maxGrappleDistance = 35f;
    [Tooltip("Reticle color when a valid grapple surface is under crosshair.")]
    [SerializeField] private Color reticleValid = Color.cyan;
    [Tooltip("Reticle color when nothing valid is under crosshair.")]
    [SerializeField] private Color reticleInvalid = Color.white;
    [Tooltip("Image that tints when a valid target is under the crosshair.")]
    [SerializeField] private Image reticleImage;

    // Internal aiming
    private bool hasValidTarget = false;
    private Vector3 lastHitPoint = Vector3.zero;
    private Vector3 lastHitNormal = Vector3.up;

    private void OnEnable()
    {
        aimCamera = GetComponent<Camera>();
    }
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

        if (aimCamera == null)
        {
            Debug.LogWarning("[Grapple]: Aim Camera is not assigned");
            hasValidTarget = false;
        }

        Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);

        // 2) Raycast against allowed layers within max distance
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleLayers, QueryTriggerInteraction.Ignore))
        {
            // We hit a valid grapple surface
            hasValidTarget = true;
            lastHitPoint = hit.point;
            lastHitNormal = hit.normal;

            // Optional: draw a small gizmo line in the Scene view for debugging
            Debug.DrawLine(ray.origin, hit.point, Color.cyan);
            Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.yellow);

            // Reticle feedback
            TintReticle(true);

            // 3) Attach only if a valid target exists AND player clicks attach
            if (Input.GetKeyDown(attachKey))
            {
                ChangeState(GrappleState.Attached);
            }
        }
        else
        {
            // No valid target under crosshair
            hasValidTarget = false;
            TintReticle(false);
            Debug.DrawRay(ray.origin, ray.direction * maxGrappleDistance, Color.gray);
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

    private void TintReticle(bool valid)
    {
        if (reticleImage == null) return;
        reticleImage.color = valid ? reticleValid : reticleInvalid;
    }
}
