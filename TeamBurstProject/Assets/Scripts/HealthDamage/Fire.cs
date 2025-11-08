using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public GameObject flamingObject;
    public int damage;
    public float timeBetweenDamage;
    [SerializeField] float spreadRadius;
    [SerializeField] LayerMask spreadLayers;
    [SerializeField] Vector2 durationMinMax;
    [SerializeField][Range(0, 1)] float spreadChance;
    float damageTimer;
    float lifeTimer;

    private void Start()
    {
        lifeTimer = Random.Range(durationMinMax.x, durationMinMax.y);
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0) Die();

        transform.position = flamingObject.transform.position;

        damageTimer += Time.deltaTime;
        if (damageTimer > timeBetweenDamage)
        {
            flamingObject.GetComponent<Flammable>().damageScript.TakeDamage(damage);
            damageTimer = 0;
            Collider[] hits = Physics.OverlapSphere(transform.position, spreadRadius);
            foreach(Collider _nextHit in hits)
            {
                Flammable _nextFlammable = _nextHit.GetComponent<Flammable>();
                if(_nextFlammable != null && !_nextFlammable.isOnFire)
                {
                    _nextFlammable.Ignite(true);
                    Fire newFire = Instantiate(this);
                    newFire.flamingObject = _nextHit.gameObject;
                }
            }
        }
    }

    private void Die()
    {
        flamingObject.GetComponent<Flammable>().isOnFire = false;
        Destroy(gameObject);
    }
}
