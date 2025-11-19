using UnityEngine;

public class DestructableCue : MonoBehaviour
{
    public AudioClip destructableCue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SoundManager.Instance != null && destructableCue != null)
            {
                SoundManager.Instance.PlayEffect(destructableCue, 1);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //do nothing

        }

    }
}
