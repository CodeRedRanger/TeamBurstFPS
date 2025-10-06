using UnityEngine;

public class itemPickup : MonoBehaviour, IPickup
{
    [SerializeField] ItemData item;
    [SerializeField] bool destroyOnPickup;

    public void Pickup()
    {
        Hotbar hotbar = FindAnyObjectByType<Hotbar>();
        if (hotbar.Add(item, 1))
        {
            if (destroyOnPickup) Destroy(gameObject);
        }
    }
}
