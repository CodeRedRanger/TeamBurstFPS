using UnityEngine;

public class LaunchPadBoss : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //just needed to mark the boss of this level; 
        gameManager.instance.launchpadBossKilled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
