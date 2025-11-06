using UnityEngine;

public class FiniteState : MonoBehaviour
{
    protected FiniteStateMachine fsMachine;
    protected bool isCurrentState;
    public virtual void OnUpdate() { }

    public virtual void OnEnter(FiniteStateMachine _calledByMachine) { fsMachine = _calledByMachine; isCurrentState = true; }

    public virtual void OnExit() { isCurrentState = false; }
}
