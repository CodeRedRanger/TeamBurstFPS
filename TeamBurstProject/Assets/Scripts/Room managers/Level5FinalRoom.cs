using UnityEngine;

public class Level5FinalRoom : MonoBehaviour
{
    GameObject objectToDisappear; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.UpdateSoldiersKilled(1); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
