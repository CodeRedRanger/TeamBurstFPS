using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EnemySpawnerRobb : MonoBehaviour
{
    //[SerializeField] int minEnemiesPerSpawn;
    //[SerializeField] int maxEnemiesPerSpawn;
    [SerializeField] float minSpawnInterval;
    [SerializeField] float maxSpawnInterval;
    //[SerializeField] float spawnRadius;
    [SerializeField] int spawnCycles;
    [SerializeField] float diameterOfSpawnedObject;
    [SerializeField] GameObject otherSpawner1;
    [SerializeField] GameObject otherSpawner2;
    [SerializeField] GameObject otherSpawner3;
    [SerializeField] float spawnerInterval; 
    //make an array of other spawners to update for new location
    private List<GameObject> otherSpawners;
    public GameObject EnemyPrefab;
    //private float spawnYLevel = 0f; 
    private int currentCycle = 0;
    //private int maxSpawnAttempts = 10;
    //private int spawnAttempts = 0;
    //private bool positionFound = false;
    private int randomSpawnLocations = 0;
    private Vector3 SpawnerPosition; 
    float spawnRadius = 0; 
    

    void Start()
    {

        otherSpawners = new List<GameObject>();
        if (otherSpawner1 != null && otherSpawner2 != null && otherSpawner3 != null)
        {
            if (otherSpawner1 != null)
            {
                otherSpawners.Add(otherSpawner1);
                
            }

            if (otherSpawner2 != null)
            {
                otherSpawners.Add(otherSpawner2);
                
            }

            if (otherSpawner3 != null)
            {
                otherSpawners.Add(otherSpawner3);
                
            }


        }
      

        randomSpawnLocations = otherSpawners.Count; 

        spawnRadius = diameterOfSpawnedObject + 0.1f;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        float interval = Random.Range(minSpawnInterval, maxSpawnInterval);

        for (int i = -1; i != randomSpawnLocations; i++)
        {
            if (i == -1)
            {
                SpawnerPosition = gameObject.transform.position;
            }
            else
            {
                
                SpawnerPosition = otherSpawners[i].transform.position;
            }

            while (currentCycle < spawnCycles)
             {
                    // while (spawnAttempts < maxSpawnAttempts && !positionFound)
                    {
                        yield return new WaitForSeconds(interval);

                        //int enemiesSpawnSize = Random.Range(minEnemiesPerSpawn, maxEnemiesPerSpawn);
                        Vector3 randomPoint = Random.insideUnitSphere;

                        //for (int i = 0; i < enemiesSpawnSize; i++)
                        // {

                        randomPoint.y = 0;
                        randomPoint *= spawnRadius;
                        Vector3 spawnPosition = SpawnerPosition + randomPoint;
                        spawnPosition.y = gameObject.transform.position.y;

                        //Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, 5.0f); // radiusOfSpawnedObject);

                        // if (hitColliders.Length == 0)
                        //  {
                        //  positionFound = true;
                        Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
                        //   }
                        //   else
                        //   {
                        //       spawnAttempts++;
                        //   }

                        // }
                    }

                    currentCycle++;
             }

            yield return new WaitForSeconds(spawnerInterval);
            currentCycle = 0;
        }
    }
}



