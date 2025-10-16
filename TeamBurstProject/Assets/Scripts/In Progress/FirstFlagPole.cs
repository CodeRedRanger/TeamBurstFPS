using UnityEngine;

public class FirstFlagPole : MonoBehaviour
{
    //Stops sound effects triggered on the playground
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.Level1 = false;
    }
}
