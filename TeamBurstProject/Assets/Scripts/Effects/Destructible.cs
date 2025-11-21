using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Destructible : MonoBehaviour, IDamage
{

    /* README
     * This script is meant to be as extensible as possible with plug and play functionality in the editor
     * To add new functionality, please write new functions under the "Damage Events" or "Destroyed Events" sections of the script.
     * The default functionality has been added as an if statement for when there is no other event specified in the inspector to preserve existing enemies
     */

    [SerializeField] int maxHP;
    [SerializeField] GameObject objectToDestroy;
    int currentHP;
    [SerializeField] UnityEvent destroyedEvent;
    [SerializeField] UnityEvent takeDamageEvent;

    [Header("extra fields")]
    //Robb added
    [SerializeField] Renderer model;
    [SerializeField] Renderer model2;
    private Color colorOrig;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip destroySound;
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
        currentHP -= amount;

        if (takeDamageEvent.GetPersistentEventCount() > 0)
            takeDamageEvent.Invoke();
        else
            //SimpleDamageFlash();
            RobbDamage(); 

        if (currentHP <= 0)
        {
            if (destroyedEvent.GetPersistentEventCount() > 0)
                destroyedEvent.Invoke();
            else
                RobbDestroy();
        }
    }



    // Damage Events

    public void SimpleDamageFlash()
    {
        StartCoroutine(flashDamage());
    }

    public void PlayDamageSound()
    {
        if(damageSound != null) 
            SoundManager.Instance.PlayEffect(damageSound, 1f);
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
    public void RobbDamage()
    {
        StartCoroutine(flashDamage());
        PlayDamageSound();
    }



    // Destroyed Events

    public void SimpleDestroy()
    {
        Destroy(objectToDestroy);
    }

    public void RobbDestroy()
    {
        if (damagedObject != null)
        {
            damagedObject.SetActive(true);
        }

        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        if (destroySound != null)
            SoundManager.Instance.PlayEffect(destroySound, 1f);

        SimpleDestroy();
    }
}
