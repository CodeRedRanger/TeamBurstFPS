using UnityEngine;

public class Flammable : MonoBehaviour
{
    [HideInInspector] public bool isOnFire;
    public IDamage damageScript;
    [SerializeField] public ParticleSystem flameParticles;
    [Tooltip("Chance of combustion from nearby fire")][Range(0, 1)] public float flammability = 0.75f;
    public Vector2 durationMinMax = new Vector2(2,4);
    public float spreadRadius = 3;



    private void Start()
    {
        damageScript = GetComponent<IDamage>();
    }

    public void Ignite(bool _shouldIgnite)
    {
        isOnFire = _shouldIgnite;
        flameParticles.gameObject.SetActive(_shouldIgnite);
    }
}
