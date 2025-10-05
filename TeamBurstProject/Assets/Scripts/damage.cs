using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{

    enum DamageType { moving, stationary, DOT, homing }
    [SerializeField] DamageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    //bullets don't hit anything, so below is the cleanup
    [SerializeField] int destroyTime;

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
        //could also say if type not DOT
        if (dmg != null && (type == DamageType.moving || type == DamageType.stationary || type == DamageType.homing))
        {
            dmg.TakeDamage(damageAmount);
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
