using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int minEnemiesPerSpawn;
    [SerializeField] int maxEnemiesPerSpawn;
    [SerializeField] float minSpawnInterval;
    [SerializeField] float maxSpawnInterval;
    [SerializeField] float spawnRadius;
    public GameObject EnemyPrefab;
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            int enemiesSpawnSize = Random.Range(minEnemiesPerSpawn, maxEnemiesPerSpawn);

            for (int i = 0; i < enemiesSpawnSize; i++)
            {
                Vector2 spawnOffset = Random.insideUnitCircle * spawnRadius;
                Vector2 spawnPosition = (Vector2)gameObject.transform.position + spawnOffset;

                Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
