using System.Collections.Generic;
using UnityEngine;

public class EnemyGravityLift : MonoBehaviour
{
    [SerializeField] float detectRadius;
    [SerializeField] float liftForce;
    [SerializeField] LayerMask liftLayer;

    Rigidbody[] nearbyObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, liftLayer); 
        List<Rigidbody> found = new List<Rigidbody>();

        foreach (Collider collider in hits)
        {
            Rigidbody rb = collider.attachedRigidbody;

            if(rb != null)
            {
                found.Add(rb);
                Debug.Log("Detected:" + rb.name);

                rb.AddForce(Vector3.up * liftForce * Time.deltaTime, ForceMode.Acceleration);
            }
        }
        nearbyObjects = found.ToArray();
    }
}
