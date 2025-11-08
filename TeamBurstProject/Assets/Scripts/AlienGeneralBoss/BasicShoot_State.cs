using UnityEngine;

public class BasicShoot_State : FiniteState
{
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    Vector3 playerDir;
    

    public override void OnEnter(FiniteStateMachine _calledByMachine) {
        base.OnEnter(_calledByMachine); 
    }
    public override void OnUpdate()
    {
        faceTarget();

        // Shoot basic bullet at player
        ShootPlayer();

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void ShootPlayer()
    {
        Transform boss = transform.parent;

        Instantiate(bullet, shootPos.position, boss.transform.rotation);
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
