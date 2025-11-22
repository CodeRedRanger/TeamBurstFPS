using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] Transform spawnPoint;
    [SerializeField] AudioClip spawnSound; 
    
    public void Spawn()
    {

        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        if (spawnSound != null)
        {
            SoundManager.Instance.PlayEffect(spawnSound, 0.3f);
        }

        
    }
}
