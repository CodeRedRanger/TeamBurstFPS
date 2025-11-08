using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;

    float spawnTimer;
    int spawnCount;
    bool startSpawning;

    List<int> posList = new List<int>();
    bool uniquePosition = false; 


    void Start()
    {
        
    }

    void Update()
    {
        if(startSpawning)
        {
            spawnTimer += Time.deltaTime;
            if(spawnCount < numToSpawn && spawnTimer >= spawnRate)
            {
                spawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

   /* private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); 
        }
    }*/
    public void StartSpawning()
    {
        startSpawning = true;
    }

    public void ResetSpawning()
    {
        spawnCount = 0;
        startSpawning = false;
        uniquePosition = false;
        posList = new List<int>();
    }

    private void spawn()
    {
        //fill up the position list. 
        //check if the array position equals any position in the list
        //once list size = size of transform spawnPos array
        //clear the list and start over

        int arrayPos = Random.Range(0, spawnPos.Length);
        int maxSpawnTries = 10;
        int spawnTries = 0;

      
         for (int i = 0; i != posList.Count; i++)
         {
             if (arrayPos == posList[i] && spawnTries < maxSpawnTries)
              {
                 arrayPos = Random.Range(0, spawnPos.Length);
                 i = 0;
                 spawnTries++;
             }

             else if (i == posList.Count - 1)
             {
                uniquePosition = true;
             }

         }
        

        if (uniquePosition || posList.Count == 0)
        {
            Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
            spawnCount++;
            spawnTimer = 0;
            uniquePosition = false;
            posList.Add(arrayPos); 
            if (posList.Count == spawnPos.Length)
            {
                posList.Clear(); 
            }

        }

    }
}
