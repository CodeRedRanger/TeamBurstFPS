using UnityEngine;


public class ItemPickupRobb : MonoBehaviour, IPickup
{

    //[SerializeField] ItemData item;
    [SerializeField] bool destroyOnPickup;//only chosen if not a health item
    [SerializeField] AudioClip pickupSound;




    public void Pickup()
    {
        //Hotbar hotbar = FindAnyObjectByType<Hotbar>();
      //  if (hotbar.Add(item, 1))
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









