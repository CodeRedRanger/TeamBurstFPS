using UnityEngine;
using System.Collections; 

public class checkpoint : MonoBehaviour
{
    [SerializeField] Renderer model;
    Color colorOrg;

    private void Start()
    {
        Color color = model.material.color;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(checkpointFeedback());
        }
    }

    IEnumerator checkpointFeedback()
    {
        gameManager.instance.checkpointPopup.SetActive(true);
        model.material.color = Color.red; 
        yield return new WaitForSeconds(0.5f);
        model.material.color = colorOrg;
        gameManager.instance.checkpointPopup.SetActive(false);  
    }

}
