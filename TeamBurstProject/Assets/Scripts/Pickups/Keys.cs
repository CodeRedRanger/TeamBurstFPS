using UnityEngine;

public class Keys : MonoBehaviour

{

    [SerializeField] AudioClip pickupSound; 
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
            SoundManager.Instance.PlayEffect(pickupSound, 1); 
            gameManager.instance.updateKeysCollected(1);
            Destroy(gameObject);
            
        }
    }


}
