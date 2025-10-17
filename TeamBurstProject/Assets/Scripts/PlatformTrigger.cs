using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] Transform platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.gameObject.transform.parent = platform;
        Debug.Log("Triggered");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.gameObject.transform.parent = null;
    }
}
