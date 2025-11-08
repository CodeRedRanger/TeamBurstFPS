using UnityEngine;

public class AppearObjectWithLever : MonoBehaviour
{

    public GameObject objectToAppear;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectAppear()
    {
        objectToAppear.SetActive(true);
    }
}
