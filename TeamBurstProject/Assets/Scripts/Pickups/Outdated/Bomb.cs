using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        HashSet<IDamage> damagedEnemies = new HashSet<IDamage>();

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player")) continue;

            IDamage damageable = hit.transform.GetComponent<IDamage>();
            if (damageable != null && !damagedEnemies.Contains(damageable))
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance <= explosionRadius)
                {
                    damageable.TakeDamage(damage);
                    damagedEnemies.Add(damageable);
                }
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