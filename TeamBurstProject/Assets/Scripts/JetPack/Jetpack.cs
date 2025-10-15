using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [Header("Thrust Settings")]
    [Tooltip("How fast the player moves upward when the jetpack is active.")]
    [SerializeField] private float thrustSpeed = 6f;

    [Tooltip("The maximum upward speed to prevent flying too fast.")]
    [SerializeField] private float maxUpwardSpeed = 10f;

    [Header("Fuel Settings")]
    [Tooltip("Maximum amount of fuel the jetpack can hold.")]
    [SerializeField] private float maxFuel = 3f;

    [Tooltip("How much fuel is used per second while thrusting.")]
    [SerializeField] private float burnRate = 1f;

    [Tooltip("How quickly fuel refills per second when on the ground.")]
    [SerializeField] private float regenRate = 0.7f;

    [Header("Input")]
    [Tooltip("Key used to activate the jetpack.")]
    [SerializeField] private KeyCode jetpackKey = KeyCode.Space;

    [Header("Ground Check")]
    [Tooltip("Transform at the character's feet used to detect the ground.")]
    [SerializeField] private Transform groundCheck;

    [Tooltip("How far below the groundCheck we check for the ground.")]
    [SerializeField] private float groundCheckDistance = 0.3f;

    [Tooltip("Which layers count as ground.")]
    [SerializeField] private LayerMask groundMask;

    [Header("Feedback (Optional)")]
    [Tooltip("ParticleSystem that plays while thrusting.")]
    [SerializeField] private ParticleSystem jetpackFX;

    [Tooltip("AudioSource that plays while thrusting.")]
    [SerializeField] private AudioSource jetpackAudio;

    //Runtime Variables
    // Stores the current amount of fuel.
    private float fuel;

    // Whether we are thrusting right now.
    private bool isThrusting = false;

    // Used to track when thrust starts/stops (for FX).
    private bool wasThrustingLastFrame = false;


    private void Awake()
    {
        // Awake() runs once when the object first loads.

        // 1) Set our starting fuel to full.
        fuel = maxFuel;

        // 2) Make sure any jetpack FX or sounds are stopped at the beginning.
        if (jetpackFX != null) jetpackFX.Stop();
        if (jetpackAudio != null) jetpackAudio.Stop();
    }

    public float GetFuel01()
    {
        return (maxFuel > 0f) ? Mathf.Clamp01(fuel / maxFuel) : 0f;
    }

    /// <summary>
    /// Adds fuel (for pickups, etc.). Keeps value between 0 and max.
    /// </summary>
    public void RefillFuel(float amount)
    {
        fuel = Mathf.Clamp(fuel + amount, 0f, maxFuel);
    }

    /// <summary>
    /// Sets the fuel to a specific percent of max (0 to 1).
    /// </summary>
    public void SetFuelPercent(float pct01)
    {
        fuel = Mathf.Clamp01(pct01) * maxFuel;
    }
}

}
