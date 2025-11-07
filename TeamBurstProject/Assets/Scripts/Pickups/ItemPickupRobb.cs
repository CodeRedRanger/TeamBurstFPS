using System.Collections; 
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
        //WILL HANDLE ADDING KEY TO THE INVENTORY LATER...MAY BE ABLE TO ADD TO HOTBAR TOO BUT HAVE TO TRACK NUMBER OF KEYS
        if (!healthItem && item.type != ItemType.key)
        {
            //not condition is so you can't pick up same item twice, since items now persist between levels
            if (item.type == ItemType.bomb) // && !gameManager.instance.enableBomb)
            {
                gameManager.instance.enableBomb = true;
                gameManager.instance.flashBombUI = true;
                gameManager.instance.flashItemUI(); 
            }
            if (item.type == ItemType.grenade && !gameManager.instance.enableGrenade)
            {
                gameManager.instance.enableGrenade = true;
                gameManager.instance.flashGrenadeUI = true;
                gameManager.instance.flashItemUI();
            }
            if (item.type == ItemType.stunner && !gameManager.instance.enableStunner)
            {
                gameManager.instance.enableStunner = true;
                gameManager.instance.flashStunnerUI = true;
                gameManager.instance.flashItemUI();
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









