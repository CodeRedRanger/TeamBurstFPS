using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeTraps : MonoBehaviour
{
    [SerializeField] private bool autoTriggered = false; //, retractOnExit = true, isDud = false;

    public Animator spikeAnim;

    public Collider activationZone;
    public float raiseDelay = 0f, lowerDelay = 3f;

    private bool isActive=false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
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

    private void OnEnable()
    {
        if (autoTriggered) StartCoroutine(LoopingCycle());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("Player")) StartCoroutine(OpenCloseTrap());
    }

    private IEnumerator LoopingCycle()
    {
        //repeat forever
        while (autoTriggered)
        {
            yield return StartCoroutine(OpenCloseTrap());
        }
    }

    private IEnumerator OpenCloseTrap()
    {
        if (!isActive)
        {
            isActive = true;
            //Wait for Raise Delay
            yield return new WaitForSeconds(raiseDelay);
            
            //Raise The Trap
            spikeAnim.SetTrigger("open");

            //Wait for Lower Delay
            yield return new WaitForSeconds(lowerDelay);
            spikeAnim.SetTrigger("close");
            isActive = false;
        }
        

    }
}
