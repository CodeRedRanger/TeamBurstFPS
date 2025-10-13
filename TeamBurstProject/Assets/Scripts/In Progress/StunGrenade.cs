using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StunGrenade : MonoBehaviour
{
    [SerializeField] float explosionRadius;
    [SerializeField] float stunDuration;
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
        HashSet<IStunnable> stunnedEnemies = new HashSet<IStunnable>();

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player")) continue;

            IStunnable stunnable = hit.transform.GetComponent<IStunnable>();
            if (stunnable != null && !stunnedEnemies.Contains(stunnable))
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance <= explosionRadius)
                {
                    stunnable.Stun(stunDuration);
                    stunnedEnemies.Add(stunnable);
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
