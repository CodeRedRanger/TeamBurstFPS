using NUnit.Framework.Constraints;
using UnityEngine;

public class Climb : MonoBehaviour
{

    [SerializeField] float climbSpeed = 15;
    [SerializeField] bool autoClimb;
    [SerializeField] bool stickToClimb = true;
    //[SerializeField] bool isRope;
 
    bool onLadder;
    int speedOrig;
    int gravOrig;
    int sprintSpeed; 
    bool wasSprinting; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedOrig = gameManager.instance.player.GetComponent<PlayerController>().speed;
        gravOrig = gameManager.instance.player.GetComponent<PlayerController>().getGravity();
        sprintSpeed = gameManager.instance.player.GetComponent<PlayerController>().speed *
            gameManager.instance.player.GetComponent<PlayerController>().sprintMod; 
        //Debug.Log(speedOrig);
    }

    // Update is called once per frame
    void Update()
    {
        if (onLadder)
        {
            move();
        }

        if(onLadder && Input.GetButtonDown("Jump"))
        {
            dismount();
        }

        if(onLadder && Input.GetKey(KeyCode.S) && gameManager.instance.player.GetComponent<CharacterController>().isGrounded)
        {
            dismount();
        }
    }

    void move()
    {
        PlayerController playerController = gameManager.instance.player.GetComponent<PlayerController>();
        if (Input.GetKey(KeyCode.W))
        {
            playerController.playerVel.y = (climbSpeed * 100f) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && stickToClimb)
        {
            playerController.playerVel.y = (-climbSpeed * 100f) * Time.deltaTime;
        }
        else if(!autoClimb && stickToClimb)
        {
            playerController.playerVel.y = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter");
        if (other.CompareTag("Player"))
        {
            

            if (other.TryGetComponent<Jetpack>(out Jetpack jp))
            {
                jp.enabled = false; //then reenable on exit
                //Debug.Log("disable jetpack pickup");
            }
            else
            {
                //Debug.Log("no jetpack assigned");
            }
            PlayerController playerController = other.GetComponent<PlayerController>();
            CharacterController characterController = other.GetComponent<CharacterController>();
            wasSprinting = playerController.isSprinting;

            playerController.speed = 0;
            if (stickToClimb)
            {
                playerController.setGravity(0);
            }

            //if (!isRope)
            //{
            //    Vector3 newPos = new Vector3(gameObject.transform.position.x, characterController.transform.position.y, characterController.transform.position.z);
            //    characterController.transform.position = newPos;
            //}

            onLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dismount();
            if (other.TryGetComponent<Jetpack>(out Jetpack jp))
                {
                jp.GetComponent<Jetpack>().enabled = true; //then reenable on exit
                //Debug.Log("enable jetpack pickup");
            }
            else
            {
                //Debug.Log("no jetpack assigned");
            }
        }
    }

    void dismount()
    {
        gameManager.instance.player.GetComponent<PlayerController>().speed = wasSprinting ? speedOrig * gameManager.instance.player.GetComponent<PlayerController>().sprintMod : speedOrig;
        gameManager.instance.player.GetComponent<PlayerController>().setGravity(gravOrig);
        //Debug.Log(gameManager.instance.player.GetComponent<PlayerController>().speed);
        onLadder = false;
    }
}
