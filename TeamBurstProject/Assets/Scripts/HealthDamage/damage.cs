using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{

    enum DamageType { moving, stationary, DOT, homing, medkit }

    //implement later with freeze ray and electric gun, plus fire gun
    //enum EffectType { none, slow, stun, knockback }

    [SerializeField] DamageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int healAmount; //for medkit
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    //bullets don't hit anything, so below is the cleanup
    [SerializeField] int destroyTime;
    [SerializeField] AudioClip itemSound;
  

//used with DOT damage type
bool isDamaging;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //look to see if a bullet
        if (type == DamageType.moving || type == DamageType.homing)
        {

            Destroy(gameObject, destroyTime);

            if (type == DamageType.moving)
            {
                //just setting so don't need time.deltatime (only use in update)
                rb.linearVelocity = transform.forward * speed;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == DamageType.homing)
        {
            //must normalize the vector to get direction only (magnitude of 1)
            rb.linearVelocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;


        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return; //ignore other triggers

        

        IDamage dmg = other.GetComponent<IDamage>();

        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered trigger with " + name);
        }
        
        //could also say if type not DOT
        if (dmg != null && (type == DamageType.moving || type == DamageType.stationary || type == DamageType.homing))
        {
            dmg.TakeDamage(damageAmount);
        }

        if (dmg != null && type == DamageType.medkit)
        {
            if (itemSound != null)
            {
                SoundManager.Instance.PlayEffect(itemSound);
            }

            dmg.Heal(healAmount);
            Destroy(gameObject);
        }

        if (type == DamageType.moving || type == DamageType.homing)
        {
            Destroy(gameObject);
        }
    }

    //trigger is a collider you can enter (use for bullets and lava)
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return; //ignore other triggers

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == DamageType.DOT)
        {
            if (!isDamaging)
            {
                StartCoroutine(damageOther(dmg));
            }
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.TakeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;


    }

}
