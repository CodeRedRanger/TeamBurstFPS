using UnityEngine;


public class ItemPickupRobb : MonoBehaviour, IPickup
{

    [SerializeField] ItemData item;
    [SerializeField] bool destroyOnPickup;//only chosen if not a health item
    [SerializeField] bool healthItem; 
   
    [SerializeField] AudioClip pickupSound;

    bool hasBeenPickedUp = false; // To prevent multiple pickups
   



    public void Pickup()
    {

        //Debug.Log("Picking up " + item.type);
        if (hasBeenPickedUp)
        {
             return; 
        }

        hasBeenPickedUp = true;
        //Debug.Log("Picking up " + item.type);

        //probably should have separate health pick up script if no health items are added to inventory
        if (!healthItem)
        {
            if (item.type == ItemType.bomb)
            {
                gameManager.instance.enableBomb = true;
            }
            if (item.type == ItemType.grenade)
            {
                gameManager.instance.enableGrenade = true;
            }
            if (item.type == ItemType.stunner)
            {
                gameManager.instance.enableStunner = true;
            }


            //Debug.Log("Adding " + item.type + " to inventory!");
            InventoryManager.Instance.AddItem(item);

          

        }
        
        //if (hotbar.Add(item, 1))
        {
          

            if (destroyOnPickup && !healthItem)
            {            
                if (pickupSound != null)
                {
                    SoundManager.Instance.PlayEffect(pickupSound, 1);
                }

                Destroy(gameObject);
            }
        }
    }



}









