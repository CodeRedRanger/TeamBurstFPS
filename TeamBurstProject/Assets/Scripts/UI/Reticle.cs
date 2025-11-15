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
    public Image fireRateIndicator;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (fireRateIndicator.enabled)
        {
            UpdateFireRateIndicator();
        }
    }

    public void UpdateFireRateIndicator()
    {
        if (currentGunData != null && gameManager.instance.playerScript.shootTimer < currentGunData.shootRate && !ammoDepletedReticle.enabled)
        {
            fireRateIndicator.transform.localScale = new Vector3(gameManager.instance.playerScript.shootTimer / currentGunData.shootRate, 1, 1);
            fireRateIndicator.enabled = true;
        }
        else
            fireRateIndicator.enabled = false;
    }

    public void PlayShoot()
    {
        Refresh();
        if (currentGunData != null && currentGunData.ammoCur > 0)
        {
            PlayAnimation("Shoot", currentGunData.reticleAnimSpeed);
            if(currentGunData.showFireRateIndicator && !ammoDepletedReticle.enabled) fireRateIndicator.enabled = true;
        }
    }

    public void PlayReload()
    {
        fixedReticle.enabled = false;
        dynamicReticle.enabled = false;
        ammoDepletedReticle.enabled = false;
        reloadReticle.enabled = true;
        PlayAnimation("Reload", currentGunData.reloadSpeed);
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
        if (currentGunData == null) TryFetchGunData();
        if (currentGunData == null) return;
        bool _hasAmmo = currentGunData.ammoCur > 0;
        // set correct reticles visible
        gameManager.instance.SetAmmoIcon(gameManager.instance.playerScript.gunList[gameManager.instance.playerScript.gunListPos].ammoIcon);
        fireRateIndicator.enabled = true;
        reloadReticle.enabled = false;
        fixedReticle.enabled = _hasAmmo;
        dynamicReticle.enabled = _hasAmmo;
        ammoDepletedReticle.enabled = !_hasAmmo;
    }

    public void TryFetchGunData()
    {
        PlayerController _playerScript = gameManager.instance.playerScript;
        if (_playerScript.gunList.Count > 0)
        {
            SetGunData(_playerScript.gunList[_playerScript.gunListPos]);
        }
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

    public void Hide()
    {
        fireRateIndicator.enabled = false;
        dynamicReticle.enabled = false;
        fixedReticle.enabled = false;
        hitReticle.enabled = false;
        reloadReticle.enabled = false;
        ammoDepletedReticle.enabled = false;
    }

    public bool IsReloading()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Reload");
    }
}

