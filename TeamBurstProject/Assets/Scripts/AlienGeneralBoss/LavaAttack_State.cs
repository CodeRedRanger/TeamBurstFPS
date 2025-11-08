using UnityEngine;

public class LavaAttack_State : FiniteState
{
    [SerializeField] FiniteState nextState;
    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        Debug.Log("Entered Lava Attack!");
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
