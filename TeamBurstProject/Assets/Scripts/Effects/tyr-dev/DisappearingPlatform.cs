using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] bool disappearOnStep = false, startHidden = false;
    [SerializeField] float visibleTime = 3.0f, hiddenTime = 2.0f, fadeDuration = 0.5f;

    private Renderer[] renderers;
    private Collider[] colliders;
    private Color[] originalColors;

    private bool isRunningCycle = false;

    private void Awake()
    {
        // Get the Renderer so we can turn the platform's visibility on/off
        renderers = GetComponentsInChildren<Renderer>();

        // Get the Collider so we can enable/disable standing on the platform
        colliders = GetComponentsInChildren<Collider>();

        // Warn in the Console if required components are missing so you know what to add
        if (renderers == null || renderers.Length == 0)
        {
            Debug.LogWarning("DisappearingPlatform: No Renderer found. Add a MeshRenderer to this platform.");
        }

        if (colliders == null || colliders.Length == 0)
        {
            Debug.LogWarning("DisappearingPlatform: No Collider found. Add a Collider (like BoxCollider) to this platform.");
        }

        // Save the starting colors for each renderer so we can restore alpha later
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            // We read the color from the first material on the renderer
            // This is simple and beginner friendly (advanced setups can have multiple materials)
            originalColors[i] = renderers[i].material.color;
        }
    }

    private void OnEnable()
    {
        // If we should start hidden, set our visuals to fully transparent and disable collider
        if (startHidden)
        {
            SetAllMaterialsAlpha(0f);
            setColliders(false);
        }
        else
        {
            
        }

        // If we are not waiting for the player, start the automatic loop
        if (!disappearOnStep && !isRunningCycle)
        {
            StartCoroutine(LoopingCycle());
        }
    }
    private void setColliders(bool state)
    {
        foreach (var collider in colliders) { collider.enabled = state; }
    }
    private IEnumerator LoopingCycle()
    {
        // Mark that a cycle is running so we do not start another one by mistake
        isRunningCycle = true;
        while (true)
        {
            // Stay visible for the chosen time
            yield return new WaitForSeconds(visibleTime);

            // Hide the platform so it cannot be used
            HidePlatform();

            // Stay hidden for the chosen time
            yield return new WaitForSeconds(hiddenTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Only do anything if we are using the "disappear when stepped on" mode
        if (!disappearOnStep) return;

        // Check if the thing that entered the trigger is the player (by tag)
        if (other.CompareTag("Player") && !isRunningCycle)
        {
            // Start the disappear/reappear routine one time
            StartCoroutine(StepTriggeredCycle());
        }
    }
    private IEnumerator StepTriggeredCycle()
    {
        // Mark that we are running a cycle so we do not start it twice
        isRunningCycle = true;

        // Keep the platform visible/solid for the chosen time after the player steps on it
        yield return new WaitForSeconds(visibleTime);

        // Hide the platform so the player falls if they are still on it
       

        // Keep it hidden for the chosen time
        yield return new WaitForSeconds(hiddenTime);

        // Show the platform again so it can be used another time
        

        // Mark that the cycle is finished so the platform can respond again later
        isRunningCycle = false;
    }

    private void SetAllMaterialsAlpha(float alpha)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            // Read the current color from the material
            Color c = renderers[i].material.color;

            // Keep the same RGB (red, green, blue) values but replace the alpha
            c.a = alpha;

            // Write the new color back to the material
            renderers[i].material.color = c;
        }
    }
}
