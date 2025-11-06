using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPickupGun
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    public int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed; 
    [SerializeField] float maxJumpSpeed;
    private float origMaxJumpSpeed;
    [SerializeField] int jumpCountMax;
    [SerializeField] int gravity; 

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] float invincibleDur;

    [SerializeField] ParticleSystem ps;
    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;

    private Vector3 moveDir;
    //changed below from private to public for jetpack
    public Vector3 playerVel;  
     

    int jumpCount;
    int HPOrig;

    float shootTimer;

    bool isSprinting;
    bool isInvincible;


    //Audio
    //can make these arrays
    //public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip deathSound;

    //Jump audio
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    //steps audio
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    bool isPlayingSteps;
    //recharge audio
    [SerializeField] AudioClip audRechargePrompt;
    [Range(0, 1)][SerializeField] float audRechargePromptVol;

    [SerializeField] public List<GunData> gunList = new List<GunData>();
    [SerializeField] public GameObject gunModel;

    [HideInInspector] public int gunListPos;
    //[HideInInspector] public bool hasPistol;
    //[HideInInspector] public bool hasSMG;
    //[HideInInspector] public bool hasCannon;
    //[HideInInspector] public bool hasFlameThrower;

    //pushback
    public Vector3 pushBack;
    [SerializeField] int pushBackTime;

    //UI feedback
    bool gainHealth = false;
    bool loseHealth = false;

    // For Variable Jump
    bool isMaxJumpSpeed = false;
 

    //For gravity boots to work with variable jump
    [HideInInspector] public bool gravityFlipped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        HPOrig = HP;
        //updatePlayerUI(); //called in spawn player
        spawnPlayer();
        origMaxJumpSpeed = maxJumpSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);

        shootTimer += Time.deltaTime;

        if (!gameManager.instance.isPaused)
        {
            Movement();
        }

        Sprint();

        SpawnBomb();
    }

    //Added for jetpack and anti-gravity boots
    public void setGravity(int _gravity) { gravity = _gravity; }
    public void setJumpSpeed(int _jumpSpeed) { jumpSpeed = _jumpSpeed; }

    public int getGravity() { return gravity; }
    public int getJumpSpeed() { return jumpSpeed; }


    //pushback
    public void AppliedPushBack(Vector3 direction)
    {
        pushBack = direction;
    }

    void Movement()
    {
        //pushback
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);


        if (controller.isGrounded)
        {
            if (moveDir.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }

        //pushback
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move((moveDir + pushBack) * speed * Time.deltaTime);

        Jump();
        controller.Move(playerVel * Time.deltaTime);

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            Shoot();

        }
        selectGun();
        reload();
    }
    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }
    void Jump()
    {
        
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax)
        {
            SoundManager.Instance.PlayEffect(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            
            if (gravityFlipped)
            {
                maxJumpSpeed = -maxJumpSpeed;
            }
            else
            {
                maxJumpSpeed = origMaxJumpSpeed;
            }


                playerVel.y = jumpSpeed;
            jumpCount++;

            isMaxJumpSpeed = false;
        }

        if (Input.GetButton("Jump")  && !isMaxJumpSpeed)
        {
            // this magic number can not be < 1
            if(gravityFlipped)
            {   
                playerVel.y -= 2;
            }
            else
                playerVel.y += 2;

            if (playerVel.y > maxJumpSpeed)
                isMaxJumpSpeed = true;
        }

        if (Input.GetButtonUp("Jump"))
            isMaxJumpSpeed = true;

    }

    void Shoot()
    {
        shootTimer = 0;

        //I added this if statement to lecture code
        if (gunList.Count > 0 && gunList[gunListPos].ammoCur > 0)
        {
            gunList[gunListPos].ammoCur--;

            if (gunList[gunListPos].ammoCur == 0)
            {
                SoundManager.Instance.PlayEffect(audRechargePrompt, audRechargePromptVol);

            }

            updatePlayerUI();
            SoundManager.Instance.PlayEffect(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootSoundVol);
            //SoundManager.Instance.PlayEffect(shootSound, 1);

            if (gunList[gunListPos].type == GunType.smg)
            {
                ps1.Play();
            }

            else if (gunList[gunListPos].type == GunType.cannon)
            {
                ps2.Play();
            }
            else
            {
                ps.Play();
            }


            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
            {
                Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.TakeDamage(shootDamage);
                }

                //Debug.Log(hit.collider.name);
            }
        }
    }

    void SpawnBomb()
    {
      
        if (Input.GetKeyDown(KeyCode.E) && gameManager.instance.enableBomb == true)
        {
            Vector3 spawnPos = gameManager.instance.player.transform.position;
            spawnPos.y -= gameManager.instance.player.GetComponent<CharacterController>().height / 2f;
            this.GetComponent<BombSpawner>().SpawnBomb(spawnPos);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {       
            HP -= damage;
            loseHealth = true; 
            updatePlayerUI();
            loseHealth = false;   
            StartCoroutine(flashDamage());
            SoundManager.Instance.PlayEffect(damageSound, 1);
            StartCoroutine(invincible());

            if (HP <= 0)
            {
                SoundManager.Instance.PlayEffect(deathSound, 1);
                //not needed anymore because reducing sound instead. 
                //SoundManager.Instance.StopMusic();
                //Debug.Log("You are dead"); 
                gameManager.instance.youLose();

            }
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBarUp.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerHPBarDown.fillAmount = (float)HP / HPOrig;

        if (loseHealth || gainHealth)
        {
            StartCoroutine(flashHPBarChange());
        }
        
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        

        if (gunList.Count > 0)
        {
            gameManager.instance.ammoCur.text = gunList[gunListPos].ammoCur.ToString("F0");
            gameManager.instance.ammoMax.text = gunList[gunListPos].ammoMax.ToString("F0");
        }
    }

    //Isaac scripts
    public void AddShootDamage(int amount)
    {
        shootDamage += amount;
    }

    public void AddJumpSpeed(int amount)
    {
        int prev = jumpSpeed;

        jumpSpeed += amount;
    }

    public int GetJumpCountMax()
    {
        return jumpCountMax;
    }

    public void SetJumpCountMax(int count)
    {
        jumpCountMax = count;
    }

    public void SpeedBoost(int amt)
    {

        speed += amt;

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
        gainHealth = true;
        updatePlayerUI();
        gainHealth = false;
    }

    void reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
            //I added to lecture code
            updatePlayerUI();
        }
    }

    public void getGunData(GunData gun)
    {
        if ((gun.type == GunType.pistol && gameManager.instance.hasPistol == true) || 
            (gun.type == GunType.smg && gameManager.instance.hasSMG == true) ||
            (gun.type == GunType.cannon && gameManager.instance.hasCannon == true) ||
            (gun.type == GunType.flamethrower && gameManager.instance.hasFlameThrower == true))
        {
            return; //already have this gun, do nothing
        }

        gunList.Add(gun);


        gunListPos = gunList.Count - 1;

        if (gun.type == GunType.pistol)
        {
            gameManager.instance.hasPistol = true;
        }
        else if (gun.type == GunType.smg)
        {
            gameManager.instance.hasSMG = true;
        }
        else if (gun.type == GunType.cannon)
        {
            gameManager.instance.hasCannon = true;
        }
        else if (gun.type == GunType.flamethrower)
        {
            gameManager.instance.hasFlameThrower = true;
        }

        changeGun();
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
        {
            gunListPos++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
        }


    }

    void changeGun()
    {
        shootDamage = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;


        updatePlayerUI();
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;
        {
            SoundManager.Instance.PlayEffect(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);
        }
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingSteps = false;

    }
    public void spawnPlayer()
    {
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;
        HP = HPOrig;
        updatePlayerUI();
    }


    IEnumerator flashHPBarChange()
    {
        if (loseHealth)
        {
            gameManager.instance.playerHPBarDown.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameManager.instance.playerHPBarDown.gameObject.SetActive(false);
        }

        if (gainHealth)
        {
            gameManager.instance.playerHPBarUp.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameManager.instance.playerHPBarUp.gameObject.SetActive(false);
        }
        
    }

    
    public void CheckIfGrounded()
    {
        if (controller.isGrounded)
        {
            if (moveDir.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }
    }

    public void resetJump()
    {
        {
            if (moveDir.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
    }

    public void SetInvincibility(bool state)
    {
        isInvincible = state;
    }

    IEnumerator invincible()
    {
        isInvincible = true;
        //enable visual indicator here
        yield return new WaitForSeconds(invincibleDur);
        //disable visual indicator here
        isInvincible = false;
    }

    public void instantDeath()
    {
        isInvincible = false;
        TakeDamage(HP);
    }
    
}