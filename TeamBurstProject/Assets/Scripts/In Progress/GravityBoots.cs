using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GravityBoots : MonoBehaviour
{
    [SerializeField] int rotSpeed;
    [SerializeField] KeyCode useKey;
    
    private bool flipping;
    private bool flipped;
    private float rotated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(useKey) && !flipping)
            flipping = true;

        if (flipping)
            FlipGravity();

        //if(isGrounded())
        //{
        //    gameObject.GetComponent<PlayerController>().jumpCount = 0;
        //}
    }

    void FlipGravity()
    {
        transform.localRotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
        //cam.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        rotated += rotSpeed * Time.deltaTime;

        if (rotated >= 180f)
        {
            flipped = !flipped;
            if (flipped)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 180);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            rotated = 0f;
            flipping = false;
        }
    }

    //bool isGrounded()
    //{
    //    RaycastHit hit;
    //    return Physics.SphereCast(transform.position, 0.5f, transform.up, out hit, (transform.localScale.y/2) + 0.2f);
    //}
}
