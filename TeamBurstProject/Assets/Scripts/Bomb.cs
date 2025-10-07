using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float explosionRadius;
    [SerializeField] float timer;
    [SerializeField] GameObject explosionEffect;

    void Start()
    {
        StartCoroutine(BombTimer());
    }


    void Explode()
    {

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player")) continue; 

            IDamage damageable = hit.GetComponent<IDamage>();
            if (damageable != null)
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance <= explosionRadius)
                    damageable.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(timer);
        Explode();
    }
}