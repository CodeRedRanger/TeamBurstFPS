using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] bool disappearOnStep = false, startHidden = false;
    [SerializeField] float visibleTime = 3.0f, hiddenTime = 2.0f;

    private Renderer meshRenderer;
    private Collider col;

    private bool isRunningCycle = false;

    private void Awake()
    {
        // Get the Renderer so we can turn the platform's visibility on/off
        meshRenderer = GetComponent<Renderer>();

        // Get the Collider so we can enable/disable standing on the platform
        col = GetComponent<Collider>();

        // Warn in the Console if required components are missing so you know what to add
        if (meshRenderer == null)
        {
            Debug.LogWarning("DisappearingPlatform: No Renderer found. Add a MeshRenderer to this platform.");
        }

        if (col == null)
        {
            Debug.LogWarning("DisappearingPlatform: No Collider found. Add a Collider (like BoxCollider) to this platform.");
        }
    }

    private void OnEnable()
    {
        // Set initial state based on startHidden
        if (startHidden)
        {
            HidePlatform();
        }
        else
        {
            ShowPlatform();
        }

        // If we want the platform to loop by itself, start the looping routine now
        if (!disappearOnStep && !isRunningCycle)
        {
            StartCoroutine(LoopingCycle());
        }
    }

    private void HidePlatform()
    {
        // Turn off the visual part if it exists
        if (meshRenderer != null) meshRenderer.enabled = false;

        // Turn off the collider so the player falls through
        if (col != null) col.enabled = false;
    }
    private void ShowPlatform() 
    {
        if (meshRenderer != null) meshRenderer.enabled = true;
        if (col != null) col.enabled = true;
    }
    private IEnumerator LoopingCycle()
    {
        // Mark that a cycle is running so we do not start another one by mistake
        isRunningCycle = true;
        while (true)
        {
            // Make sure the platform is visible and solid
            ShowPlatform();

            // Stay visible for the chosen time
            yield return new WaitForSeconds(visibleTime);

            // Hide the platform so it cannot be used
            HidePlatform();

            // Stay hidden for the chosen time
            yield return new WaitForSeconds(hiddenTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Only react in the "disappear on step" mode
        if (!disappearOnStep) return;

        // Check if the thing that touched us is the player (the player should have the "Player" tag)
        if (collision.collider.CompareTag("Player"))
        {
            // Start the disappear/reappear routine if it's not already running
            if (!isRunningCycle)
            {
                StartCoroutine(StepTriggeredCycle());
            }
        }
    }
    private IEnumerator StepTriggeredCycle() 
    {
        // Mark that we are running a cycle so we do not start it twice
        isRunningCycle = true;

        // Keep the platform visible/solid for the chosen time after the player steps on it
        yield return new WaitForSeconds(visibleTime);

        // Hide the platform so the player falls if they are still on it
        HidePlatform();

        // Keep it hidden for the chosen time
        yield return new WaitForSeconds(hiddenTime);

        // Show the platform again so it can be used another time
        ShowPlatform();

        // Mark that the cycle is finished so the platform can respond again later
        isRunningCycle = false;
    }
}
