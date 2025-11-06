using UnityEngine;

public class PlayAnimation_State : FiniteState
{
    [SerializeField] Animator anim;
    [SerializeField] string animationToPlay;
    [SerializeField] FiniteState nextState;
    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        anim.Play(animationToPlay);
    }

    public void MoveToNextState()
    {
        fsMachine.ChangeToState(nextState);
    }
}
