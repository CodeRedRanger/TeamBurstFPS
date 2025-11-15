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
    private bool isInvulnerable = false;

    void Start()
    {
        colorOrig = model.material.color;
    }
    public void Heal(int amount)
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable)
            return;

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

    public void MakeInvulnerable()
    {
        isInvulnerable = true;
        gameObject.GetComponentInChildren<Renderer>().material.color = new Color(0, 0, 1);
    }

    public void MakeVulnerable()
    {
        isInvulnerable = false;
        gameObject.GetComponentInChildren<Renderer>().material.color = colorOrig;
        Debug.Log("Called MakeVulnerable");
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
