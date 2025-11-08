using UnityEngine;

public class SpinShoot_State : FiniteState
{
    [SerializeField] FiniteState nextState;
    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        Debug.Log("Entered Spin Shoot State");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.T))
        {
            fsMachine.ChangeToState(nextState);
        }
    }
}
