using System.Collections;
using Unity.XR.GoogleVr;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] bool disappearOnStep = false, startHidden = false;
    [SerializeField] float visibleTime = 3.0f, hiddenTime = 2.0f, fadeDuration = 0.5f;

    [SerializeField] private AudioClip fadeOutClip;
    [SerializeField] private AudioClip fadeInClip;
    [SerializeField] private float volume = 1f;

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
            SetColliders(false);
        }
        else
        {
            SetAllMaterialsAlpha(1f);
            SetColliders(true);
            
        }

        // If we are not waiting for the player, start the automatic loop
        if (!disappearOnStep && !isRunningCycle)
        {
            StartCoroutine(LoopingCycle());
        }
    }
    private void SetColliders(bool state)
    {
        foreach (var collider in colliders) { collider.enabled = state; }
    }
    private IEnumerator LoopingCycle()
    {
        isRunningCycle = true;

        // Repeat forever
        while (true)
        {
            // Ensure fully visible and solid before waiting
            CallAudio(fadeInClip);
            yield return StartCoroutine(FadeToAlpha(1f, fadeDuration));
            SetColliders(true);
            yield return new WaitForSeconds(visibleTime);

            // Fade to invisible and then disable the collider
            CallAudio(fadeOutClip);
            yield return StartCoroutine(FadeToAlpha(0f, fadeDuration));
            SetColliders(false);
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

        // Make sure we are fully visible and solid for the chosen time
        yield return StartCoroutine(FadeToAlpha(1f, fadeDuration));
        SetColliders(true);
        yield return new WaitForSeconds(visibleTime);

        // Fade out to fully invisible, then disable the collider
        yield return StartCoroutine(FadeToAlpha(0f, fadeDuration));
        SetColliders(false);

        // Stay invisible for the chosen time
        yield return new WaitForSeconds(hiddenTime);

        // Fade back in to fully visible, then re-enable the collider
        yield return StartCoroutine(FadeToAlpha(1f, fadeDuration));
        SetColliders(true);

        isRunningCycle = false;
    }
    private IEnumerator FadeToAlpha(float alpha, float duration) 
    {
        // Make sure duration is never zero or negative to avoid division problems
        float time = Mathf.Max(0.0001f, duration);

        // For each renderer, remember where we start so we can blend to the target
        float[] startAlphas = new float[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            startAlphas[i] = renderers[i].material.color.a;
        }

        // t goes from 0 to 1 over "time" seconds
        float t = 0f;
        while (t < 1f)
        {
            // Increase t based on how much time passed since last frame
            t += Time.deltaTime / time;

            // Lerp (blend) each renderer's alpha from start to target
            for (int i = 0; i < renderers.Length; i++)
            {
                // Read current color
                Color c = renderers[i].material.color;

                // Blend from the start alpha to the target alpha
                c.a = Mathf.Lerp(startAlphas[i], Mathf.Clamp01(alpha), t);

                // Write color back to the material
                renderers[i].material.color = c;
            }

            // Wait until the next frame and continue the loop
            yield return null;
        }

        // After the loop, force the exact final alpha value (avoids tiny rounding errors)
        SetAllMaterialsAlpha(alpha);
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

    private void CallAudio(AudioClip input) 
    {
        SoundManager.Instance.PlayEffect(input, volume);
    }
}
