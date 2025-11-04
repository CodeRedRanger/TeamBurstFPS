using UnityEngine;

public class FiniteState : MonoBehaviour
{
    protected FiniteStateMachine fsMachine;
    public virtual void OnUpdate() { }

    public virtual void OnEnter(FiniteStateMachine _calledByMachine) { fsMachine = _calledByMachine; }

    public virtual void OnExit() { }
}
