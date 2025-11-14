using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Pit : MonoBehaviour
{
    [SerializeField] float killPlayerDelay = 0.5f;
    [SerializeField] AudioClip playerFalling;
    [Range(0,1)][SerializeField] float playerFallingVol = 0.5f;
    [SerializeField] AudioClip objectFalling;
    [Range(0, 1)][SerializeField] float objectFallingVol = 0.5f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(killPlayer());
        }
        else if (other.GetComponent<Rigidbody>())
        {
            StartCoroutine(destroyObject(other.gameObject));
        }
    }

    IEnumerator killPlayer()
    {
        SoundManager.Instance.PlayEffect(playerFalling, playerFallingVol);
        yield return new WaitForSeconds(killPlayerDelay);
        gameManager.instance.player.GetComponent<PlayerController>().instantDeath();
    }

    IEnumerator destroyObject(GameObject other)
    {
        AudioSource tempAudSource = other.AddComponent<AudioSource>();
        tempAudSource.spatialBlend = 1f;
        tempAudSource.PlayOneShot(objectFalling, objectFallingVol);
        yield return new WaitForSeconds(objectFalling.length);
        Destroy(other);
    }
        
}
