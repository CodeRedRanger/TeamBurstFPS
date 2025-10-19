using Unity.Mathematics;
using UnityEngine;
using System.Collections; 


public class DoorController : MonoBehaviour
{
    public GameObject doorHinge;
    //public float openAngle = 90f;
    //public float closeAngle = 0f;
    public float rotationSpeed = 2f;

    private bool isOpen = false; 


    //[SerializeField] Transform doorHinge;
    //[SerializeField] float speed;
    //[SerializeField] float CloseTime;

    //private bool isOpen;
    //private float openAngle;
    //private float currentAngle;
    //private float openTimer;

    //float targetAngle; 

    public void OpenDoor(float openAngle)
    {
        if(!isOpen)
        {
            StopAllCoroutines();
            StartCoroutine(RotateDoor(openAngle));
            isOpen = true; 
        }
        
    }

    public void CloseDoor(float closeAngle)
    {
        if(isOpen)
        {
            StopAllCoroutines();
            StartCoroutine(RotateDoor(closeAngle));
            isOpen = false;
        }
       
    }

    IEnumerator RotateDoor(float targetAngle)
    {
   
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        while (Quaternion.Angle(doorHinge.transform.localRotation, targetRotation) > 0.01f)
        {
            doorHinge.transform.localRotation = Quaternion.Lerp(doorHinge.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        doorHinge.transform.localRotation = targetRotation;
       
    }



    //public void OpenDoor(float angle)
    //{
    //    if (!isOpen)
    //    {
    //        isOpen = true;
    //        openAngle = angle;
    //        openTimer = 0;
    //    }

    //    targetAngle = openAngle; 

    //    if (currentAngle != targetAngle)
    //    {
    //        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.deltaTime);
    //        doorHinge.localRotation = Quaternion.Euler(0, currentAngle, 0);
    //    }

    //    if (isOpen)
    //    {
    //        openTimer += Time.deltaTime;
    //        if (openTimer >= CloseTime)
    //        {
    //            CloseDoor();
    //        }
    //    }



    //}

    //public void CloseDoor()
    //{
    //    if (isOpen)
    //    {
    //        targetAngle = 0;
    //        isOpen = false;
    //    }
    //}
}
