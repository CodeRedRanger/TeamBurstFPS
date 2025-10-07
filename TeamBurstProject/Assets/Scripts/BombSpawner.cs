using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] KeyCode key;
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Vector3 spawnPos = gameManager.instance.player.transform.position;
            spawnPos.y -= gameManager.instance.player.GetComponent<CharacterController>().height / 2f;
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }
    }
}
