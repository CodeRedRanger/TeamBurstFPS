using UnityEngine;

public class BasicShoot_State : FiniteState
{
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Transform headPos;

    Vector3 playerDir;
    

    public override void OnEnter(FiniteStateMachine _calledByMachine) {
        base.OnEnter(_calledByMachine); 
    }
    public override void OnUpdate()
    {
        faceTarget();

        // Shoot basic bullet at player

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    void faceTarget()
    {
        Transform boss = transform.parent;
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, boss.transform.position.y, playerDir.z));
        rot.z = 0;
        Debug.Log(rot);
        boss.transform.rotation = Quaternion.Lerp(boss.transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
}
