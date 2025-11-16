using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] int mouseSens;
    [SerializeField] int vertLimitLow, vertLimitHigh;

    [SerializeField] bool invertY;

    float rotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        //Get raw mouse input
        //float mouseX_raw = Input.GetAxis("Mouse X");
        //float mouseY_raw = Input.GetAxis("Mouse Y");

        //Can make variable public float maxDeltaPerFrame = 10;f; 
        //mouseX_raw = Mathf.Clamp(mouseX_raw, -10, 10);
        //mouseY_raw = Mathf.Clamp(mouseY_raw, -10, 10);



        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        //float mouseX = mouseX_raw * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * Time.deltaTime;
        //float mouseY = mouseY_raw * mouseSens * Time.deltaTime;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        rotX = Mathf.Clamp(rotX, vertLimitLow, vertLimitHigh);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);

    }


}
