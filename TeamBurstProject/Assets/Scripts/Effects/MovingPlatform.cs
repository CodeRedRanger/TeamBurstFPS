using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform desination;
    [SerializeField] Transform platform;
    [SerializeField] float movementSpeed;
    
    Vector3 startingPos;
    bool isTravelingForward;

    private void Start()
    {
        startingPos = transform.position;
        isTravelingForward = true;
    }

    private void Update()
    {
        Vector3 newPos;

        if (isTravelingForward)
        {
            newPos = Vector3.MoveTowards(platform.position, desination.position, movementSpeed * Time.deltaTime);
            if (CheckIfDoneTravling(desination.position))
                isTravelingForward = false;
        }
        else
        {
            newPos = Vector3.MoveTowards(platform.position, startingPos, movementSpeed * Time.deltaTime);
            if (CheckIfDoneTravling(startingPos))
                isTravelingForward = true;
        }

        platform.position = newPos;
    }

    private bool CheckIfDoneTravling(Vector3 pointOfInterest)
    {
        float distance = Vector3.Distance(platform.position, pointOfInterest);

        if (distance <= 0.01)
            return true;
        else
            return false;
    }
}
