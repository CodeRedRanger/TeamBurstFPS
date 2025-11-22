using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;

    public void SpawnBomb(Vector3 spawnPos)
    {
        if (gameManager.instance.numberBombsGrenades < 5)
        {
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            //update numberBombGrenade +1
            gameManager.instance.UpdateNumberBombsGrenades(1);
        }
    }
}
