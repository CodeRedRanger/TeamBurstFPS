using UnityEngine;

public class PlatformInactiveLever : MonoBehaviour
{
    public GameObject platform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InactivatePlatform()
    {
        platform.GetComponent<MovingPlatform>().enabled = false;
    }
}
