using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GravityBoots : MonoBehaviour
{
    [SerializeField] int rotSpeed;
    [SerializeField] KeyCode useKey;
    [SerializeField] bool rotatePlayer;
    
    private bool flipping;
    private bool gravityFlipped;
    private float rotated;
    private bool bootsActivated = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame


    void Update()
    {
        if (bootsActivated)
        {
            if (Input.GetKeyDown(useKey) && !flipping)
            {
                if (rotatePlayer)
                {
                    flipping = true;
                }
                else
                {
                    FlipGravity();
                }
            }


            if (rotatePlayer && flipping)
                FlipPlayerAndGrav();


            if (gravityFlipped && isGrounded())
            {
                gameManager.instance.player.GetComponent<PlayerController>().resetJump();
            }
        }
    }

    void FlipPlayerAndGrav()
    {
        GameObject player = gameManager.instance.player;
        PlayerController playerScript = gameManager.instance.playerScript;
        player.transform.localRotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
        rotated += rotSpeed * Time.deltaTime;

        if (rotated >= 180f)
        {
            if (!gravityFlipped)
            {
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 180);;
            }
            else
            {
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
            }

            FlipGravity();

            flipping = false;
            
            rotated = 0f;
        }
    }

    void FlipGravity()
    {
        PlayerController playerScript = gameManager.instance.playerScript;

        playerScript.setGravity(-playerScript.getGravity());
        playerScript.setJumpSpeed(-playerScript.getJumpSpeed());

        gravityFlipped = !gravityFlipped;
    }

    bool isGrounded()
    {
        GameObject player = gameManager.instance.player;
        Vector3 spherePos = player.transform.position;
        spherePos.y += (player.GetComponent<CharacterController>().height / 2) - 0.42f;

        float radius = player.GetComponent<CharacterController>().radius;

        int mask = LayerMask.GetMask("Default");

        return Physics.CheckSphere(spherePos, radius, mask);
    }

    public void ResetGravity()
    {
        if (gravityFlipped)
        {
            FlipGravity();
            if (rotatePlayer)
            {
                GameObject player = gameManager.instance.player;
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
            }
            //flipping = false;
            //gravityFlipped = false;
            //rotated = 0f;

        }
        else if (flipping)
        {
            flipping = false;
            rotated = 0f;
            GameObject player = gameManager.instance.player;
            player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) 
        {
           bootsActivated = true;
        }

    }

    //void OnDrawGizmosSelected()
    //{
    //    Vector3 spherePos = gameManager.instance.player.transform.position;
    //    spherePos.y += (gameManager.instance.player.GetComponent<CharacterController>().height / 2) - 0.42f;

    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawWireSphere(spherePos, gameManager.instance.player.GetComponent<CharacterController>().radius);
    //}
}
