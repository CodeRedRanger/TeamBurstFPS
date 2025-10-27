using UnityEngine;

public class Rotator : MonoBehaviour
{
   
    public float rotationSpeed = 90f;

    [Header("Rotation Axes")]
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationVector = Vector3.zero;
        if (rotateX)
        {
            rotationVector += Vector3.right;
        }

        if (rotateY)
        {
            rotationVector += Vector3.up;
        }

        if (rotateZ)
        {
            rotationVector += Vector3.forward;
        }

        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
    }
}
