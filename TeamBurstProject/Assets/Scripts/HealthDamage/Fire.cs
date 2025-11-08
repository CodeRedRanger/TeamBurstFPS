using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public GameObject flamingObject = null;
    [HideInInspector] public Flammable flamingScript;
    public int damage;
    public float timeBetweenDamage;
    [SerializeField] LayerMask spreadLayers;
    float damageTimer;
    float lifeTimer = 10;
    bool hasIgnited;

    public void Ignite(GameObject _flammableObject)
    {
        flamingObject = _flammableObject;
        transform.parent = _flammableObject.transform;
        transform.position = transform.parent.position;
        flamingScript = flamingObject.GetComponent<Flammable>();
        flamingScript.Ignite(true);
        lifeTimer = Random.Range(flamingScript.durationMinMax.x, flamingScript.durationMinMax.y);
        hasIgnited = true;
    }

    private void Update()
    {
        if(!hasIgnited) return;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0) Die();

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
        Collider[] hits = Physics.OverlapSphere(transform.position, flamingScript.spreadRadius, spreadLayers);
        foreach (Collider _nextHit in hits)
        {
            Flammable _nextFlammable = _nextHit.GetComponent<Flammable>();
            if (_nextFlammable != null && !_nextFlammable.isOnFire && Random.Range(0f,1f) <= _nextFlammable.flammability)
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
