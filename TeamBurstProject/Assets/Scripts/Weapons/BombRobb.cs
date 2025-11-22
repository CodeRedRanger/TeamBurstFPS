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

            IDamage damageable = hit.transform.GetComponent<IDamage>();
            //if (damageable == null)
            //    Debug.Log("Damageable is null!"); 

            if (damageable != null && !damagedEnemies.Contains(damageable))
            {
                //Debug.Log("Damaging");
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance <= explosionRadius || (hit.CompareTag("UFO") && distance <= 20))
                {
                    damageable.TakeDamage(damage);
                    damagedEnemies.Add(damageable);

                    //if (explosionSound != null)
                      //  SoundManager.Instance.PlayEffect(explosionSound, 1);
                }
            }
        }
        if (explosionSound != null)
            SoundManager.Instance.PlayEffect(explosionSound, 1);

        //Update numberBombGrenade -1
        gameManager.instance.UpdateNumberBombsGrenades(-1);

        Destroy(gameObject);
    }

    IEnumerator BombTimer()
    {
       
        Renderer [] bombRenderer = GetComponentsInChildren<Renderer>();
        Color origColor;  

        if(bombRenderer != null)
        {
            foreach (Renderer renderer in bombRenderer)
                origColor = renderer.material.color;
            
            yield return new WaitForSeconds(timer * 0.75f);
            
            foreach (Renderer renderer in bombRenderer)
                renderer.material.color = Color.red;
            
            yield return new WaitForSeconds(timer * 0.25f);

        }
        else 
        {
            yield return new WaitForSeconds(timer);
        }
        
        Explode();
    }
}
