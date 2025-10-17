using NUnit.Framework.Interfaces;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunData gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickupGun pickupable = other.GetComponent<IPickupGun>();

        if (pickupable != null)
        {
            gun.ammoCur = gun.ammoMax;
            pickupable.getGunData(gun);
            Destroy(gameObject);
        }

    }
}
