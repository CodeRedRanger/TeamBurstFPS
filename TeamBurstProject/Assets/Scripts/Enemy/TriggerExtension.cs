using UnityEngine;

public class TriggerExtension : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    ITrigger targetScript;

    private void Start()
    {
        targetScript = targetObject.GetComponent<ITrigger>();
        if(targetScript == null) Destroy(gameObject);
            
    }

    private void OnTriggerEnter(Collider _other)
    {
        targetScript.OnTriggerEnter(_other);
    }

    private void OnTriggerExit(Collider _other)
    {
        targetScript.OnTriggerExit(_other);
    }

    private void OnTriggerStay(Collider _other)
    {
        targetScript.OnTriggerStay(_other);
    }
}
