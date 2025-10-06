using UnityEngine;


public class ItemPickupRobb : MonoBehaviour, IPickup
{

    //[SerializeField] ItemData item;
    [SerializeField] bool destroyOnPickup;
    [SerializeField] AudioClip pickupSound;
    


    public void Pickup()
    {
        //Hotbar hotbar = FindAnyObjectByType<Hotbar>();
      //  if (hotbar.Add(item, 1))
        {
            if (destroyOnPickup)
            {
                Debug.Log("Picked up " + gameObject.name);
                if (pickupSound != null)
                {
                    SoundManager.Instance.PlayEffect(pickupSound);
                }
                    
                Destroy(gameObject);
            }
        }
    }



}









