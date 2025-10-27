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
        objectToThrowPrefab = null;
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Alpha2) && gameManager.instance.enableGrenade == true)
        {
            objectToThrowPrefab = grenadePrefab;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3) && gameManager.instance.enableStunner == true)
        {
            objectToThrowPrefab = stunGrenadePrefab;
        }
     


        if (Input.GetKeyDown(key) && (gameManager.instance.enableGrenade || gameManager.instance.enableStunner))
        {
            if (objectToThrowPrefab != null)
            {
                Throw();
            }
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