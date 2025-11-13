using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    public static Reticle instance;
    public Animator anim;
    public float hitReticleDuration;
    public GunData currentGunData;

    [Header("Image References")]
    public Image fixedReticle;
    public Image dynamicReticle;
    public Image ammoDepletedReticle;
    public Image reloadReticle;
    public Image hitReticle;

    private void Awake()
    {
        instance = this;
    }

    public void PlayShoot(bool _hasAmmo)
    {
        if (_hasAmmo)
        {
            PlayAnimation("Shoot", currentGunData.reticleAnimSpeed);
        }
        else
        {
            Refresh();
        }
    }

    public void PlayReload()
    {
        fixedReticle.enabled = false;
        dynamicReticle.enabled = false;
        ammoDepletedReticle.enabled = false;
        reloadReticle.enabled = true;
        PlayAnimation("Reload");
    }

    public void SetGunData(GunData _newGun)
    {
        // update reticle images
        if (_newGun != null)
        {
            currentGunData = _newGun;
            fixedReticle.sprite = currentGunData.fixedReticle;
            dynamicReticle.sprite = currentGunData.dynamicReticle;

            fixedReticle.SetNativeSize();
            dynamicReticle.SetNativeSize();
        }
        Refresh();
    }

    public void Refresh()
    {
        bool _hasAmmo = currentGunData.ammoCur > 0;
        // set correct reticles visible
        reloadReticle.enabled = false;
        fixedReticle.enabled = _hasAmmo;
        dynamicReticle.enabled = _hasAmmo;
        ammoDepletedReticle.enabled = !_hasAmmo;
    }

    public void PlayHit()
    {
        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        hitReticle.enabled = true;
        float timer = hitReticleDuration;
        //float coroutineProgress;
        while (timer > 0)
        {
            // DELETE LATER (COMMENTS)
            //coroutineProgress = timer / hitReticleDuration;
            //hitReticle.color = new Color(hitReticle.color.r, hitReticle.color.g, hitReticle.color.b, coroutineProgress);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        hitReticle.enabled = false;
    }

    private void PlayAnimation(string _name, float _speed = 1f)
    {
        anim.speed = _speed;
        anim.Play(_name);
    }
}

