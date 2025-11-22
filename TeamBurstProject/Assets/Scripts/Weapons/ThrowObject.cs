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
    private float playerSpeed; 
    private float origThrowForce;

    private void Start()
    {
        objectToThrowPrefab = null;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
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

        if (gameManager.instance.GetNumberBombsGrenades() < 5)
        {

            //added
            //moved player variable initialization to start
            //player = GameObject.FindGameObjectWithTag("Player");
            //playerScript = player.GetComponent<PlayerController>();
            playerSpeed = playerScript.speed;
            origThrowForce = throwForce;
            throwForce += playerSpeed;



            GameObject thrownObject = Instantiate(objectToThrowPrefab, throwPoint.position, throwPoint.rotation);

            //Update number bomb/grenades +1
            //can take out if statement after implement -1 into stun grenade explode
            //if (objectToThrowPrefab == grenadePrefab)
            gameManager.instance.UpdateNumberBombsGrenades(1);


            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            }

            throwForce = origThrowForce;
        }
    }
}