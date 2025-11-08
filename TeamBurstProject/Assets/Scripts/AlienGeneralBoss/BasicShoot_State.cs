using UnityEngine;

public class BasicShoot_State : FiniteState
{
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float fireRate;

    Vector3 playerDir;
    float shootTimer;

    public override void OnEnter(FiniteStateMachine _calledByMachine) {
        base.OnEnter(_calledByMachine);
        shootTimer = 0;
    }
    public override void OnUpdate()
    {
        shootTimer += Time.deltaTime;

        faceTarget();
        ShootPlayer();

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void ShootPlayer()
    {
        Transform boss = transform.parent;

        if (shootTimer > fireRate)
        {
            shootTimer = 0;
            Quaternion rot = Quaternion.LookRotation(gameManager.instance.player.transform.position - shootPos.transform.position);
            Instantiate(bullet, shootPos.position, rot);
        }
    }

    void faceTarget()
    {
        Transform boss = transform.parent;
        playerDir = gameManager.instance.player.transform.position - boss.transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        //rot.z = 0;
        boss.transform.rotation = Quaternion.Lerp(boss.transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
}
