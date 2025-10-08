using UnityEngine;
using System.Collections; 

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpCountMax;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    public ParticleSystem ps;

    private Vector3 moveDir;
    private Vector3 playerVel;

    int jumpCount;
    int HPOrig;

    float shootTimer;

    bool isSprinting;

    //Audio
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);

        shootTimer += Time.deltaTime;

        Movement();

        Sprint();

        SpawnBomb();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();
        controller.Move(playerVel * Time.deltaTime);

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            Shoot();
            SoundManager.Instance.PlayEffect(shootSound);
            ps.Play(); 
        }
    }
    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }

    void Shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }

            //Debug.Log(hit.collider.name);
        }
    }

    void SpawnBomb()
    {
        // Im thinking of adding a keycode variable for this but for now it's q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 spawnPos = gameManager.instance.player.transform.position;
            spawnPos.y -= gameManager.instance.player.GetComponent<CharacterController>().height / 2f;
            this.GetComponent<BombSpawner>().SpawnBomb(spawnPos);
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        updatePlayerUI(); 
        StartCoroutine(flashDamage());
        SoundManager.Instance.PlayEffect(damageSound);

        if (HP <= 0)
        {
            SoundManager.Instance.PlayEffect(deathSound); 
            SoundManager.Instance.StopMusic();
            Debug.Log("You are dead"); 
            gameManager.instance.youLose();
           
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig; 
    }

    public void AddShootDamage(int amount)
    {
        shootDamage += amount;
    }

    public void AddJumpSpeed(int amount)
    {
        int prev = jumpSpeed;

        jumpSpeed += amount;
    }

    IEnumerator flashDamage()
    {
        gameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlash.SetActive(false);
    }


    public void Heal(int amount)
    {
        HP += amount;
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        updatePlayerUI();
    }



}
