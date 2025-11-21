using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public GameObject grenadePrefab;
    public GameObject stunGrenadePrefab;
    public GameObject objectToThrowPrefab;
    public Transform throwPoint;
    public KeyCode key;
    public float throwForce;

    //added so can get player speed
    public GameObject player; //reference to player object
    public PlayerController playerScript; //reference to player script

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
        //added
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        float playerSpeed = playerScript.speed;
        float origThrowForce = throwForce; 
        throwForce += playerSpeed; 

        GameObject thrownObject = Instantiate(objectToThrowPrefab, throwPoint.position, throwPoint.rotation);

        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        }

        throwForce = origThrowForce;
    }
}