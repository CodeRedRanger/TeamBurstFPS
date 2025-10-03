using UnityEngine;

public class medkit : MonoBehaviour, IPickup
{
    [SerializeField] int healAmount;
    [SerializeField] bool destroyOnPickup;
    [SerializeField] KeyCode useKey;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && Input.GetKey(useKey) && destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            player = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player = null;
        }
    }

    public void Pickup()
    {
        
    }
}
