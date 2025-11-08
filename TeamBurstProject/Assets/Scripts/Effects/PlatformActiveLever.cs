using UnityEngine;

 
public class PlatformActiveLever : MonoBehaviour
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

    public void ActivatePlatform()
    {
        platform.GetComponent<MovingPlatform>().enabled = true;
    }



}
