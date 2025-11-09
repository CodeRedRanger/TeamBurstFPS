using UnityEngine;
using System.Collections;

public class GeneralDamage : MonoBehaviour, IDamage
{
    [SerializeField] GameObject finalGoal;
    [SerializeField] int health;
    [SerializeField] Renderer model;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;

    private Color colorOrig;

    void Start()
    {
        colorOrig = model.material.color;
    }
    public void Heal(int amount)
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            finalGoal.SetActive(true);
            SoundManager.Instance.PlayEffect(deathSound, 1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
            SoundManager.Instance.PlayEffect(damageSound, 1);
        }

    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
