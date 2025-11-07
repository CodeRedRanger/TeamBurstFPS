using UnityEngine;

public class GravityBootsCancel : MonoBehaviour
{
    [SerializeField] GameObject gravityBoots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.gravityBootsPopup.SetActive(false);
            gravityBoots.GetComponent<GravityBoots>().ResetGravity();
            gravityBoots.GetComponent<GravityBoots>().enabled = false;
        }
    }
    

}
