using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] Transform spawnPoint;
    [SerializeField] AudioSource audSource;
    [SerializeField] AudioClip spawnSound;
    private float volume; 
    
    public void Spawn()
    {

        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        if (spawnSound != null)
        {
            volume = PlayerPrefs.GetFloat("SFXVolume", volume);
            //SoundManager.Instance.PlayEffect(spawnSound, 0.3f);
            audSource.PlayOneShot(spawnSound, volume); 
        }

        
    }
}
