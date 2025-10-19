using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAIRoam : MonoBehaviour, IDamage, IStunnable
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

    //for roaming
    [SerializeField] int roamDist;
    [SerializeField] float roamPauseTime;
    float roamTimer;
    float stoppingDistOrig;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);
        animator = GetComponent<Animator>();
        stoppingDistOrig = agent.stoppingDistance; //store original stopping distance
        startingPos = transform.position; //store starting position for roaming

    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("Speed", agent.velocity.magnitude);

        shootTimer += Time.deltaTime;

        if(agent.remainingDistance <= 0.01f)
        {
            roamTimer += Time.deltaTime; //only count up if not moving
        }


        if (playerInRange && !canSeePlayer()) //added ! to canSeePlayer
        {
            //below until end of function is for roam
            checkRoam();
        }
        else if (!playerInRange)
        {
            checkRoam();
        }
    }

    //for roaming
    void checkRoam()
    {
        if (roamTimer > roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam(); 
        }
    }

    //for roaming
    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = startingPos + Random.insideUnitSphere * roamDist;
        //kept Y consistent
        ranPos.y = startingPos.y; 

        ranPos += startingPos;

        NavMeshHit hit;
        //Debug.Log(agent.areaMask); //changed 1 to agent.areaMask
        if (NavMesh.SamplePosition(ranPos, out hit, roamDist, agent.areaMask))
        {
            agent.SetDestination(hit.position);
        }


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
        Debug.DrawRay(headPos.position, playerDir, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            //Debug.Log("Enemy is hitting " + hit.collider.name); 

            if (angleToPlayer < FOV && hit.collider.CompareTag("Player"))
            {
               
                agent.SetDestination(gameManager.instance.player.transform.position);

                //changed for roam
                if (agent.remainingDistance <= stoppingDistOrig && !isStunned)    //agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (shootTimer > shootRate && !isStunned)
                {
                    SoundManager.Instance.PlayEffect(shootSound, 1);
                    shoot();
                }
                
                //for roaming
                agent.stoppingDistance = stoppingDistOrig; //reset stopping distance
                return true;
            }

        }
        //for romaing
        //set to 0 if not chasing player, stopping point will be outer edge of enemies range
        agent.stoppingDistance = 0; 

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
            agent.stoppingDistance = 0; 
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


