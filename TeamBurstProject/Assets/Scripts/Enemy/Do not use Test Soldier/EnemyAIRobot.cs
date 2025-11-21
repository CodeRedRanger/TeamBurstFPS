using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyAIRobot : MonoBehaviour, IDamage, IStunnable, ITrigger
{

    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] bool isFinalBoss;
    [SerializeField] GameObject finalGoal;
    [SerializeField] LayerMask canSeeLayers;


    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;


    [SerializeField] int FOV;
    [SerializeField] Transform headPos; 

    Color colorOrig;

    float shootTimer;
    float raycastDistance = 100;
    float angleToPlayer; 

    bool playerInRange;
    bool isStunned;

    Vector3 playerDir; 

    [SerializeField] Animator animator;
    [SerializeField] int animTransSpeed;

    [SerializeField] bool itemSpawner;
    [SerializeField] public GameObject itemToSpawn; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);
        //To keep track of total to be spawned before winning
        //comment out above and in spawner script start() put
        //gameManager.instance.updateGameGoal(numToSpawn)

        //ANIM, is this needed with set AnimLocomotion?
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        setAnimLocomotion(); 

        shootTimer += Time.deltaTime;
      

        if (playerInRange && !canSeePlayer())
        {

            //This is for roam logic, see roaming alien script for example
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

    void setAnimLocomotion()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = animator.GetFloat("Speed");
        animator.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed)); 
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
        //Debug.Log("Drawing a ray"); 
        //Debug.DrawRay(headPos.position, playerDir, Color.red, 2f);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit, raycastDistance, canSeeLayers))
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
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!playerInRange && other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void shoot()
    {
        //Debug.Log("Shooting"); 
        shootTimer = 0;


        //Instantiate(bullet, shootPos.position, transform.rotation);
        animator.SetTrigger("Shoot"); 

        
    }

    public void createBullet()
    {
        //gets enemy to aim at player's base no matter player's height 
        Vector3 playerPosition = gameManager.instance.player.transform.position;
        Vector3 directionToTarget = (playerPosition - shootPos.position).normalized;
        
        Instantiate(bullet, shootPos.position, Quaternion.LookRotation(directionToTarget));

        //original code
        //Instantiate(bullet, shootPos.position, transform.rotation);

    }

    public void TakeDamage(int amount)
    {
            
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position); //chases player if hit

        if (HP <= 0)
        {
            SoundManager.Instance.PlayEffect(deathSound, 1);

            if (TryGetComponent<Level5FinalRoom>(out Level5FinalRoom level5))
            {
                gameManager.instance.UpdateSoldiersKilled(-1);

            }

            if (TryGetComponent<LaunchPadBoss>(out LaunchPadBoss launchPadBoss))
            {
                gameManager.instance.LaunchpadBossKilled(); 
            }



            if (itemSpawner && itemToSpawn != null)
            {
                Instantiate(itemToSpawn, transform.position + Vector3.up, Quaternion.identity);
            }

            if (isFinalBoss)
            {
                if(finalGoal != null)
                    finalGoal.SetActive(true);
            }

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
        yield return new WaitForSeconds(0.3f); //0.1f
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

        yield return new WaitForSeconds(duration);

        if (agent != null) agent.isStopped = false;
        isStunned = false;
    }
}
