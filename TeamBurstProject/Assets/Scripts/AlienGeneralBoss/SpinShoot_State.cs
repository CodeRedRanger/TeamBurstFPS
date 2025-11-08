using UnityEngine;

public class SpinShoot_State : FiniteState
{
    [SerializeField] Animator anim;
    [SerializeField] string animationToPlay;
    [SerializeField] FiniteState nextState;
    [SerializeField] float timeBetweenShots;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    float shootTimer;
    int BULLETS_TO_FIRE = 20; // Be wary of modifying this value. Calculations may be off

    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        shootTimer = 0;
        Debug.Log("Entered Spin Shoot State");
        anim.Play(animationToPlay);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        shootTimer += Time.deltaTime;

        // Spawn bullets

        if (shootTimer > timeBetweenShots)
        {
            int bulletCount = 0;
            shootTimer = 0;
            Quaternion rot = Quaternion.LookRotation(gameManager.instance.player.transform.position - shootPos.transform.position);
            while (bulletCount < BULLETS_TO_FIRE)
            {
                bulletCount++;
                Instantiate(bullet, shootPos.position, rot);
                rot = Quaternion.Euler(0f, 18f, 0f) * rot;
            }
        }
    }

    public void MoveToNextState()
    {
        fsMachine.ChangeToState(nextState);
    }

    public void FastShoot()
    {

    }
}
