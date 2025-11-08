using UnityEngine;

public class Flammable : MonoBehaviour
{
    public bool isOnFire;
    [SerializeField] public IDamage damageScript;
    [SerializeField] public ParticleSystem flameParticles;

    public void Ignite(bool _shouldIgnite)
    {
        isOnFire = _shouldIgnite;
        flameParticles.gameObject.SetActive(_shouldIgnite);
    }
}
