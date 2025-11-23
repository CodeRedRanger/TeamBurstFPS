using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GeneralDamage : MonoBehaviour, IDamage
{
    [SerializeField] GameObject finalGoal;
    [SerializeField] GameObject bossLava; 
    [SerializeField] int health;
    [SerializeField] Renderer model;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;

    private Color colorOrig;
    private bool isInvulnerable = false;

    private int maxHealth;
   
    void Start()
    {
        colorOrig = model.material.color;
        maxHealth = health;
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
            //make final victim appear
            finalGoal.SetActive(true);

            //make lava go away so can reach victim
            bossLava.SetActive(false);

            //Makes all extra enemies go away and makes the game goal zero since game is won
            EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

            gameManager.instance.updateGameGoal(-(enemies.Length));

            foreach (EnemyAI enemy in enemies)
            {
                Destroy(enemy.gameObject);
                
            }

            
            EnemyAIRobot[] robots = FindObjectsByType<EnemyAIRobot>(FindObjectsSortMode.None);
            gameManager.instance.updateGameGoal(-(robots.Length));

            foreach (EnemyAIRobot robot in robots)
            { 
                Destroy(robot.gameObject); 
            }

            SoundManager.Instance.PlayEffect(deathSound, 1);
            gameManager.instance.bossHPBar.transform.parent.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
            SoundManager.Instance.PlayEffect(damageSound, 1);
            float normalizedHealth = (float)health / maxHealth;
            //Debug.Log(normalizedHealth);
            gameManager.instance.bossHPBar.GetComponent<Image>().fillAmount = normalizedHealth;
        }

    }

    public void MakeInvulnerable()
    {
        isInvulnerable = true;
        gameObject.GetComponentInChildren<Renderer>().material.color = new Color(0, 0, 1);
        gameManager.instance.bossHPBar.GetComponent<Image>().color = new Color(0, 0, 1);
    }

    public void MakeVulnerable()
    {
        isInvulnerable = false;
        gameObject.GetComponentInChildren<Renderer>().material.color = colorOrig;
        gameManager.instance.bossHPBar.GetComponent<Image>().color = new Color(1, 0, 0);
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
