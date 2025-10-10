using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform doorHinge;
    [SerializeField] float speed;
    [SerializeField] float CloseTime;
  
    private bool isOpen;
    private float openAngle;
    private float currentAngle;
    private float openTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        float targetAngle = isOpen ? openAngle : 0;

        if (currentAngle != targetAngle)
        {
            currentAngle = Mathf.MoveTowards(currentAngle,targetAngle, speed * Time.deltaTime);
            doorHinge.localRotation = Quaternion.Euler(0, currentAngle, 0);
        }

        if(isOpen)
        {
            openTimer += Time.deltaTime;
            if(openTimer >= CloseTime)
            {
                CloseDoor();
            }
        }
    }

    public void OpenDoor(float angle)
    {
        if (!isOpen)
        {
            isOpen = true;
            openAngle = angle;
            openTimer = 0;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
        }
    }
}
