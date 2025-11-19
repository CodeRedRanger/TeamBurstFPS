using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Jetpack : MonoBehaviour
{
    [Header("Thrust Settings")]
    [SerializeField] private int thrustSpeed = 50;
    [SerializeField] private int maxUpwardSpeed = 80;

    [Header("Fuel Settings")]
    [SerializeField] private int maxFuel = 100;
    [SerializeField] private int burnRate = 10;
    [SerializeField] private int regenRate = 5;

    [SerializeField] private float gravity = -9.81f; 

    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem jetpackFX;

    //added one line below
    [HideInInspector] public AudioClip jetpackClip; 
    //[SerializeField] public AudioSource jetpackAudio;


    private int fuel;
    private bool isThrusting = false;
    private bool wasThrustingLastFrame = false;
    private float verticalSpeed = 0f;

    //Restrictions
    private float maxHeight = 950f; // Maximum height the player can reach
    //private Transform playerTransform;

    private PlayerController playerScript;
    private bool isInitialized = false;


    public void Initialize(GameObject player)
    {
        playerScript = player.GetComponent<PlayerController>();
        //playerTransform = player.transform;
        fuel = maxFuel;

        if (jetpackFX != null) jetpackFX.Stop();
        //if (jetpackAudio != null) jetpackAudio.Stop();
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return; 
        bool wantsThrust = Input.GetButton("Jump");
        isThrusting = wantsThrust && fuel > 0;

        if (isThrusting)
        {
            fuel -= (int)(burnRate * Time.deltaTime);
            fuel = Mathf.Max(fuel, 0);
        }
        else
        {
            fuel += (int)(regenRate * Time.deltaTime);
            fuel = Mathf.Min(fuel, maxFuel);
        }

        if (isThrusting && fuel > 0 && !wasThrustingLastFrame)
        {
            if (jetpackFX != null) jetpackFX.Play();
            //if (jetpackAudio != null && !jetpackAudio.isPlaying) jetpackAudio.Play();
            if (jetpackClip != null) SoundManager.Instance.PlayEffect(jetpackClip, 1f);
        }

        if (!isThrusting && wasThrustingLastFrame)
        {
            if (jetpackFX != null) jetpackFX.Stop();
            //if (jetpackAudio != null && jetpackAudio.isPlaying) jetpackAudio.Stop();
            
        }

        wasThrustingLastFrame = isThrusting;
    }

    private void FixedUpdate()
    {
        if (isThrusting)
        {
            if (transform.position.y < maxHeight)
            {
                verticalSpeed += thrustSpeed * Time.fixedDeltaTime;
                verticalSpeed = Mathf.Clamp(verticalSpeed, 0, maxUpwardSpeed);
            }
            else
            {
                verticalSpeed = Mathf.Min(verticalSpeed, 0); // Prevent further upward movement
            }
        }
        else
        {
            verticalSpeed += gravity * Time.fixedDeltaTime;
        }

        if (playerScript != null)
        {
            playerScript.playerVel.y = verticalSpeed;
        }
    }

    /*
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

    // move the player vertically*/

    /*
    // ===== JETPACK SETTINGS =====
    [Header("Thrust Settings")]
    [SerializeField] private int thrustSpeed = 50;
    [SerializeField] private int maxUpwardSpeed = 80;
    [SerializeField] private float maxHeight = 50f; // Maximum height the player can reach
    [SerializeField] private int maxThrusts = 5; // Maximum number of thrust inputs allowed

    [Header("Fuel Settings")]
    [SerializeField] private int maxFuel = 100;
    [SerializeField] private int burnRate = 10;
    [SerializeField] private int regenRate = 5;

    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem jetpackFX;
    [SerializeField] public AudioSource jetpackAudio;

    public GameObject player;
    public PlayerController playerScript;

    private int fuel;
    [HideInInspector] public bool isThrusting = false;
    private bool wasThrustingLastFrame = false;
    private int verticalSpeed = 0;

    private int thrustCount = 0; // Tracks the number of thrust inputs
    private bool thrustPaused = false; // True if thrusting is paused
    private bool jetPackActive = false; 


  
    private void Awake()
    {
        
        fuel = maxFuel;
        if (jetpackFX != null) jetpackFX.Stop();
        if (jetpackAudio != null) jetpackAudio.Stop();
    }

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add the Jetpack script to the player
            if (!other.gameObject.TryGetComponent<Jetpack>(out _))
            {
                Jetpack jetpack = other.gameObject.AddComponent<Jetpack>();

                // Transfer settings to the player's jetpack
                jetpack.thrustSpeed = thrustSpeed;
                jetpack.maxUpwardSpeed = maxUpwardSpeed;
                jetpack.maxFuel = maxFuel;
                jetpack.burnRate = burnRate;
                jetpack.regenRate = regenRate;
                jetpack.jetpackFX = jetpackFX;
                jetpack.jetpackAudio = jetpackAudio;
                jetpack.jetPackActive = true; 

                // Destroy the pickup object
                Destroy(gameObject);
            }
        }
    }

    public int GetFuelPercent()
    {
        if (maxFuel <= 0) return 0;
        return Mathf.Clamp(fuel * 100 / maxFuel, 0, 100);
    }

    public void RefillFuel(int amount)
    {
        fuel = Mathf.Clamp(fuel + amount, 0, maxFuel);
    }

    public void SetFuelPercent(int percent)
    {
        fuel = Mathf.Clamp(percent, 0, 100) * maxFuel / 100;
    }

    private void Update()
    {
        if (jetPackActive)
        {
            // Check if the Jump button is held and thrusting is not paused
            bool wantsThrust = Input.GetButton("Jump") && !thrustPaused;

            // Only thrust if there is fuel and thrusting is not paused
            isThrusting = wantsThrust && fuel > 0;

            // Drain fuel when thrusting
            if (isThrusting)
            {
                fuel -= (int)(burnRate * Time.deltaTime);
                if (fuel < 0) fuel = 0;

                // Apply upward speed when thrusting
                if (player.transform.position.y < maxHeight)
                {
                    float gravityCompensation = Mathf.Abs(playerScript.getGravity() * Time.deltaTime);
                    verticalSpeed = Mathf.Clamp(verticalSpeed + (int)((thrustSpeed * Time.deltaTime) - gravityCompensation), 0, maxUpwardSpeed);
                    playerScript.playerVel.y += verticalSpeed;
                }
            }
            else
            {
                // Refill fuel when not thrusting
                fuel += (int)(regenRate * Time.deltaTime);
                if (fuel > maxFuel) fuel = maxFuel;
            }
        }

        // Start FX when thrusting begins
        if (isThrusting && !wasThrustingLastFrame)
            {
                if (jetpackFX != null) jetpackFX.Play();
                if (jetpackAudio != null && !jetpackAudio.isPlaying) jetpackAudio.Play();
            }

            // Stop FX when thrusting stops
            if (!isThrusting && wasThrustingLastFrame)
            {
                if (jetpackFX != null) jetpackFX.Stop();
                if (jetpackAudio != null && jetpackAudio.isPlaying) jetpackAudio.Stop();
            }


            // Remember thrust state for next frame
            wasThrustingLastFrame = isThrusting;
        }
    }*/

    /*  private void LateUpdate()
      {
          /*
          // Apply upward speed when thrusting
          if (isThrusting)
          {
              // Check if the player is below the maximum height
              if (player.transform.position.y < maxHeight)
              {
                  verticalSpeed = Mathf.Clamp(verticalSpeed + (int)(thrustSpeed * Time.deltaTime), 0, maxUpwardSpeed);
                  playerScript.playerVel.y += verticalSpeed;
              }
          }*/
    //}

    /* private IEnumerator ResetThrustPause()
     {
         yield return new WaitForSeconds(1f); // Pause thrusting for 3 seconds
         thrustCount = 0; // Reset thrust count
         thrustPaused = false; // Allow thrusting again
     }*/
}



