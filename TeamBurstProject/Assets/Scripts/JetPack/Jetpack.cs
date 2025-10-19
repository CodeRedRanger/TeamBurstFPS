using UnityEngine;

public class Jetpack : MonoBehaviour
{
    // ===== JETPACK SETTINGS =====
    [Header("Thrust Settings")]
    [Tooltip("How fast the player moves upward when using the jetpack (1–100).")]
    [SerializeField] private int thrustSpeed = 50;
    [Tooltip("The fastest the player can move upward (1–100).")]
    [SerializeField] private int maxUpwardSpeed = 80;

    [Header("Fuel Settings")]
    [Tooltip("Maximum amount of fuel (1–100).")]
    [SerializeField] private int maxFuel = 100;
    [Tooltip("How much fuel is burned each second while flying (1–100).")]
    [SerializeField] private int burnRate = 10;
    [Tooltip("How much fuel is regained each second when not flying (1–100).")]
    [SerializeField] private int regenRate = 5;

    [Header("Optional Effects")]
    [Tooltip("Optional particle effect (like flames).")]
    [SerializeField] private ParticleSystem jetpackFX;
    [Tooltip("Optional looping sound effect.")]
    [SerializeField] private AudioSource jetpackAudio;

    // ===== INTERNAL STATE =====
    private int fuel;                 // current amount of fuel
    private bool isThrusting = false; // true while Jump button is held and fuel > 0
    private bool wasThrustingLastFrame = false; // used for FX start/stop
    private int verticalSpeed = 0;    // upward movement speed (1–100)

    private void Awake()
    {
        // runs once when the object is created
        fuel = maxFuel; // start with full fuel
        if (jetpackFX != null) jetpackFX.Stop();   // stop particles at start
        if (jetpackAudio != null) jetpackAudio.Stop(); // stop sound at start
    }

    public int GetFuelPercent()
    {
        // returns current fuel as a percentage of max (0–100)
        if (maxFuel <= 0) return 0;
        return Mathf.Clamp(fuel * 100 / maxFuel, 0, 100);
    }

    public void RefillFuel(int amount)
    {
        // adds or removes fuel, clamped between 0 and max
        fuel = Mathf.Clamp(fuel + amount, 0, maxFuel);
    }

    public void SetFuelPercent(int percent)
    {
        // sets fuel directly based on a percent value (0–100)
        fuel = Mathf.Clamp(percent, 0, 100) * maxFuel / 100;
    }
}
