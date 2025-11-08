using UnityEngine;

public class Button : MonoBehaviour
{
    public bool buttonOn;
    Color colorOrig;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.GetComponent<Rigidbody>() != null)
        {
            buttonOn = true;
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null || other.GetComponent<Rigidbody>() != null)
        {
            buttonOn = false;
            GetComponent<Renderer>().material.color = colorOrig;
        }
    }
}
