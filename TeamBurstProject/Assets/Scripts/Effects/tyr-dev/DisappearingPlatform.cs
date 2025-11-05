using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] bool disappearOnStep = true, startHidden = false;
    [SerializeField] float visibleTime = 1.0f, hiddenTime = 1.0f;

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

}
