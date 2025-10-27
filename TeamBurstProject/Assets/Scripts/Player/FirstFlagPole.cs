using UnityEngine;

public class FirstFlagPole : MonoBehaviour
{
    //Stops sound effects triggered on the playground
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.Level1 = false;
        }
    }
}
