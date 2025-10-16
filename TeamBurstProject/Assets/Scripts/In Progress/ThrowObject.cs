using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public GameObject grenadePrefab;
    public GameObject stunGrenadePrefab;
    public GameObject objectToThrowPrefab;
    public Transform throwPoint;
    public KeyCode key;
    public float throwForce;

    private void Start()
    {
        objectToThrowPrefab = grenadePrefab;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) objectToThrowPrefab = grenadePrefab;
        if (Input.GetKeyDown(KeyCode.Alpha3)) objectToThrowPrefab = stunGrenadePrefab;
        if (Input.GetKeyDown(key)) Throw();
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