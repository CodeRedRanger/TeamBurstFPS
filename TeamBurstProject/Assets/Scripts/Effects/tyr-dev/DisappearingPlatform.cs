using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] bool disappearOnStep = true, startHidden = false;
    [SerializeField] float visibleTime = 1.0f, hiddenTime = 1.0f;

    private Renderer meshRenderer;
    private Collider col;

    private bool isRunningCycle = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
