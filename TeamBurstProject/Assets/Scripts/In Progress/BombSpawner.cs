using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;

    public void SpawnBomb(Vector3 spawnPos)
    {
        Instantiate(bombPrefab, spawnPos, Quaternion.identity);
    }
}
