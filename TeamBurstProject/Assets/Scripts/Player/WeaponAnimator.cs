using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void OnShoot()
    {
        anim.Play("Shoot");
    }
}
