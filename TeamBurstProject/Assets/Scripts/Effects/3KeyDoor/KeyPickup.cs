using UnityEngine;

public class KeyPickup : MonoBehaviour
{

    [SerializeField] KeyCode useKey = KeyCode.X;
    [SerializeField] bool touchOnly = true;
    [SerializeField] AudioClip pickupSound;
    bool inRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(touchOnly && inRange)
        {
            gameManager.instance.player.GetComponent<PlayerController>().pickupKey(1);
            SoundManager.Instance.PlayEffect(pickupSound, 1);
            Destroy(gameObject);
        }
        else if (!touchOnly && inRange && Input.GetKeyDown(useKey))
        {
            gameManager.instance.player.GetComponent<PlayerController>().pickupKey(1);
            SoundManager.Instance.PlayEffect(pickupSound, 1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickupKey pickup = other.GetComponent<IPickupKey>();
        if(pickup != null)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IPickupKey pickup = other.GetComponent<IPickupKey>();
        if (pickup != null)
        {
            inRange = false;
        }
    }
}
