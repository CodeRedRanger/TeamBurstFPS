using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Destructible : MonoBehaviour, IDamage
{
    [SerializeField] int maxHP;
    [SerializeField] GameObject objectToDestroy;

    //Robb added
    [SerializeField] Renderer model;
    [SerializeField] Renderer model2;
    private Color colorOrig;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip destroySound;

    int currentHP;
    [SerializeField] UnityEvent destroyedEvent;
    [SerializeField] UnityEvent takeDamageEvent;

    [SerializeField] ParticleSystem destroyEffect;
    [SerializeField] GameObject damagedObject;



    private void Start()
    {
        //Robb added
        colorOrig = model.material.color;

        currentHP = maxHP;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        //Robb added
        //SimpleDamageFlash(); 
        StartCoroutine(flashDamage());
        SoundManager.Instance.PlayEffect(damageSound, 1f);

        currentHP -= amount;

        if (takeDamageEvent != null)
            takeDamageEvent.Invoke();

        //Robb added: if and else wrapper
        //if (destroyedEvent != null)
        //{
        //    if (currentHP <= 0) destroyedEvent.Invoke();
        //}
        //else

        if (currentHP <= 0)
        {
            if(damagedObject != null)
            {
                damagedObject.SetActive(true);
            }

            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }

            SoundManager.Instance.PlayEffect(destroySound, 1f);
            SimpleDestroy();
        }
    }



    // Damage Events

    public void SimpleDamageFlash()
    {
        
    }

    private IEnumerator flashDamage()
    {
        model.material.color = Color.blue;
        if (model2 != null)
        {
            model2.material.color = Color.blue;
        }

        yield return new WaitForSeconds(0.3f); //0.1f
        model.material.color = colorOrig;
        if (model2 != null)
        {
            model2.material.color = colorOrig;
        }

    }



    // Destroyed Events

    public void SimpleDestroy()
    {
        Destroy(objectToDestroy);
    }
}
