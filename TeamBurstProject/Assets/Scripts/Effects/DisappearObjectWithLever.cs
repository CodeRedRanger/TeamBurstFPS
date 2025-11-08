using UnityEngine;

public class DisappearObjectWithLever : MonoBehaviour
{
    public GameObject objectToDisappear;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectDisappear()
    {
        objectToDisappear.SetActive(false);
    }
}
