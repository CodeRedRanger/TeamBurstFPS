using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRobb : MonoBehaviour
{

    [SerializeField] int damage;
    [SerializeField] float explosionRadius;
    [SerializeField] float timer;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] AudioClip explosionSound;

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

        //Debug.Log("About to explode");

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player")) continue;

            //Debug.Log("About to take damage");

            IDamage damageable = hit.transform.root.GetComponent<IDamage>();
            if (damageable != null && !damagedEnemies.Contains(damageable))
            {
                Debug.Log("Damaging");
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance <= explosionRadius)
                {
                    damageable.TakeDamage(damage);
                    damagedEnemies.Add(damageable);

                    if (explosionSound != null)
                        SoundManager.Instance.PlayEffect(explosionSound);
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
