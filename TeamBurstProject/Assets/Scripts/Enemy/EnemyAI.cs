using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage, IStunnable
{

    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip shockSound; 
    

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed; 


    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;


    [SerializeField] int FOV;
    [SerializeField] Transform headPos; 

    Color colorOrig;

    float shootTimer;

    float angleToPlayer; 

    bool playerInRange;
    bool isStunned;

    Vector3 playerDir; 

    private Animator animator;
    [SerializeField] int animTransSpeed; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);
        //To keep track of total to be spawned before winning
        //comment out above and in spawner script start() put
        //gameManager.instance.updateGameGoal(numToSpawn)
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        setAnimLocomation(); 

        shootTimer += Time.deltaTime;
      

        if (playerInRange && canSeePlayer())
        {
            /*agent.SetDestination(gameManager.instance.player.transform.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }

            if (shootTimer > shootRate)
            {
                shoot();
            }*/

        }
    }

    void setAnimLocomation()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = animator.GetFloat("Speed");

        //animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.SetFloat("Speed", Mathf.Lerp(animSpeedCur,agentSpeedCur, Time.deltaTime * animTransSpeed));
        
    
    }


    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        //Debug.DrawRay(headPos.position, playerDir, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            //Debug.Log("Enemy is hitting " + hit.collider.name); 

            if (angleToPlayer < FOV && hit.collider.CompareTag("Player"))
            {
                //adjusted this to make work compared to example
                //can try commenting out this
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance && !isStunned)
                {
                    faceTarget();
                }

                if (shootTimer > shootRate && !isStunned)
                {
                    SoundManager.Instance.PlayEffect(shootSound, 1);
                    shoot();
                }
                //comment out to here

                return true;
            }

        }

        return false; 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void TakeDamage(int amount)
    {
            
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position); //chases player if hit

        if (HP <= 0)
        {
            SoundManager.Instance.PlayEffect(deathSound, 1);
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
            
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

    public void Heal(int amount)
    {
        //not implemented for enemy
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;
        model.material.color = Color.yellow;
        SoundManager.Instance.PlayEffect(shockSound, 1);
        yield return new WaitForSeconds(duration);
        model.material.color = colorOrig;
        if (agent != null) agent.isStopped = false;
        isStunned = false;
    }
}
