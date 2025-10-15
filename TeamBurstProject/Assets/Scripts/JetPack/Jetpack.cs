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
}
