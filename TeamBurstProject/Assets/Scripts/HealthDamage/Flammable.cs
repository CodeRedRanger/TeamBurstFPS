using UnityEngine;

public class Flammable : MonoBehaviour
{
    [HideInInspector] public bool isOnFire;
    public IDamage damageScript;
    [SerializeField] public ParticleSystem flameParticles;

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
