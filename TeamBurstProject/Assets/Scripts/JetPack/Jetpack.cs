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
    [SerializeField] public AudioSource jetpackAudio;

    //[SerializeField] CharacterController controller;
    //[SerializeField] PlayerController player;

    public GameObject player; //reference to player object
    public PlayerController playerScript; //reference to player script

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        //controller = GetComponent<CharacterController>();
        //player = GetComponent<PlayerController>();
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

   
    private void Update()
    {
        // check if the Jump button is held
        bool wantsThrust = Input.GetButton("Jump") ;

        // only thrust if there is fuel
        isThrusting = wantsThrust && fuel > 0;

        // drain fuel when thrusting
        if (isThrusting)
        {
            // burnRate means "units per second", so multiply by deltaTime
            fuel -= (int)(burnRate * Time.deltaTime);
            if (fuel < 0) fuel = 0;
        }
        else
        {
            // refill fuel when not thrusting
            fuel += (int)(regenRate * Time.deltaTime);
            if (fuel > maxFuel) fuel = maxFuel;
        }

        // start FX when thrusting begins
        if (isThrusting && !wasThrustingLastFrame)
        {
            if (jetpackFX != null) jetpackFX.Play();
            if (jetpackAudio != null && !jetpackAudio.isPlaying) jetpackAudio.Play();
        }

        // stop FX when thrusting stops
        if (!isThrusting && wasThrustingLastFrame)
        {
            if (jetpackFX != null) jetpackFX.Stop();
            if (jetpackAudio != null && jetpackAudio.isPlaying) jetpackAudio.Stop();
        }

        // remember thrust state for next frame
        wasThrustingLastFrame = isThrusting;
    }

    private void LateUpdate()
    {
        // apply upward speed when thrusting
        if (isThrusting)
        {
            // increase vertical speed by thrustSpeed each second (scaled by deltaTime)
            verticalSpeed = Mathf.Clamp(verticalSpeed, verticalSpeed + (int)(thrustSpeed *Time.deltaTime), maxUpwardSpeed);
            //player.playerVel.y += verticalSpeed;
            playerScript.playerVel.y += verticalSpeed;

        }
        //This functionality is not needed as the character already has gravity applied
        /*else
        {
            // apply a simple gravity effect when not thrusting
            verticalSpeed -= (int)(thrustSpeed * Time.deltaTime);

            // limit how fast the player can go down
            if (verticalSpeed < -maxUpwardSpeed) verticalSpeed = -maxUpwardSpeed;
        }*/

        // move the player vertically


    }

}
