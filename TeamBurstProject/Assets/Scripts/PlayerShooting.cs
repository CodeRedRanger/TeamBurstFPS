using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour
{

    public float range;
    public ParticleSystem ps;
    //public GameObject particleObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range);
    }


    public void shoot()
    {
        ps.Play(); 
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            //Debug.Log("Hit " + hit.collider.name);
           // Instantiate(particleObject, hit.point, Quaternion.identity); 

        }
    }

}





