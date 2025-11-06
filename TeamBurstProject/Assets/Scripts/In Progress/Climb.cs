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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedOrig = gameManager.instance.player.GetComponent<PlayerController>().speed;
        gravOrig = gameManager.instance.player.GetComponent<PlayerController>().getGravity();
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
        Debug.Log("enter");
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            CharacterController characterController = other.GetComponent<CharacterController>();
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
        }
    }

    void dismount()
    {
        gameManager.instance.player.GetComponent<PlayerController>().speed = speedOrig;
        gameManager.instance.player.GetComponent<PlayerController>().setGravity(gravOrig);
        //Debug.Log(gameManager.instance.player.GetComponent<PlayerController>().speed);
        onLadder = false;
    }
}
