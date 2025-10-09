using UnityEngine;


public class ItemPickupRobb : MonoBehaviour, IPickup
{

    [SerializeField] ItemData item;
    [SerializeField] bool destroyOnPickup;//only chosen if not a health item
    [SerializeField] AudioClip pickupSound;

    bool hasBeenPickedUp = false; // To prevent multiple pickups





    public void Pickup()
    {
  

        if (hasBeenPickedUp)
        {
             return; 
        }

        hasBeenPickedUp = true; 
        //Debug.Log("Picking up " + item.type);

        InventoryManager.Instance.AddItem(item);
        
        //if (hotbar.Add(item, 1))
        {
          

            if (destroyOnPickup)
            {            
                if (pickupSound != null)
                {
                    SoundManager.Instance.PlayEffect(pickupSound);
                }

                Destroy(gameObject);
            }
        }
    }



}









