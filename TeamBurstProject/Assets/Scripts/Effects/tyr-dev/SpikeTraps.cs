using UnityEngine;

public class SpikeTraps : MonoBehaviour
{
    [SerializeField] private bool autoTriggered = false, retractOnExit = true, isDud = false;

    public Animator spikeAnim;

    public Collider activationZone;
    public float raiseDelay = 0f, lowerDelay = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Try to find a trigger collider under this object
        if (activationZone == null)
        {
            var colliders = GetComponentsInChildren<Collider>(true);
            foreach (var c in colliders)
            {
                if (c.isTrigger)
                {
                    activationZone = c;
                    break;
                }
            }
        }
        spikeAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
