using UnityEngine;

public class AppearDisappearTrigger : MonoBehaviour
{

    public GameObject changedObject; 




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          changedObject.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            changedObject.SetActive(false); 

        }

    }

}



