using UnityEngine;

public class PlayerPickupRobb : MonoBehaviour
{
    [SerializeField] float range;
    //[SerializeField] KeyCode useKey;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red);
        detectItem();
    }

    void detectItem()
    {
        //timer = 0;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            IPickup item = hit.collider.GetComponent<IPickup>();
            if (item != null)     // && Input.GetKeyDown(useKey))
            {
                item.Pickup();
    

                /*if (item.Pickup() == Medkit)
                {
                    add medkit to inventory/hotbar
                    OR
                    heal player
                }
                */
            }
        }
    }
}
