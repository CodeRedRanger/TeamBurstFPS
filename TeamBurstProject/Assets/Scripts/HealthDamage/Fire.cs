using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public GameObject flamingObject = null;
    public int damage;
    public float timeBetweenDamage;
    [SerializeField] float spreadRadius;
    [SerializeField] LayerMask spreadLayers;
    [SerializeField] Vector2 durationMinMax;
    [SerializeField][Range(0, 1)] float spreadChance;
    float damageTimer;
    float lifeTimer = 10;
    bool hasIgnited;

    public void Ignite(GameObject _flammableObject)
    {
        flamingObject = _flammableObject;
        transform.parent = _flammableObject.transform;
        flamingObject.GetComponent<Flammable>().Ignite(true);
        lifeTimer = Random.Range(durationMinMax.x, durationMinMax.y);
        hasIgnited = true;
    }

    private void Update()
    {
        if(!hasIgnited) return;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0) Die();

        transform.position = flamingObject.transform.position;

        damageTimer += Time.deltaTime;
        if (damageTimer > timeBetweenDamage)
        {
            flamingObject.GetComponent<Flammable>().damageScript.TakeDamage(damage);
            damageTimer = 0;
            Spread();
        }
    }

    private void Spread()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, spreadRadius, spreadLayers);
        foreach (Collider _nextHit in hits)
        {
            Flammable _nextFlammable = _nextHit.GetComponent<Flammable>();
            if (_nextFlammable != null && !_nextFlammable.isOnFire)
            {
                _nextFlammable.Ignite(true);
                Fire _newFire = Instantiate(this);
                _newFire.Ignite(_nextFlammable.gameObject);
            }
        }
    }

    private void Die()
    {
        if(flamingObject != null)
            flamingObject.GetComponent<Flammable>().Ignite(false);
        Destroy(gameObject);
    }
}
