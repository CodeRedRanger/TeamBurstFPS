using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public GameObject objectToThrowPrefab;
    public Transform throwPoint;
    public float throwForce;
    public KeyCode throwKey;

    void Update()
    {
        if (Input.GetKeyDown(throwKey))
        {
            Throw();
        }
    }

    void Throw()
    {
        GameObject thrownObject = Instantiate(objectToThrowPrefab, throwPoint.position, throwPoint.rotation);

        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        }
    }
}