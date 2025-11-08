using UnityEngine;

public class SpinShoot_State : FiniteState
{
    [SerializeField] Animator anim;
    [SerializeField] string animationToPlay;
    [SerializeField] FiniteState nextState;
    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        Debug.Log("Entered Spin Shoot State");
        anim.Play(animationToPlay);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        

        // Spawn bullets
    }

    public void MoveToNextState()
    {
        fsMachine.ChangeToState(nextState);
    }

    public void FastShoot()
    {

    }
}
