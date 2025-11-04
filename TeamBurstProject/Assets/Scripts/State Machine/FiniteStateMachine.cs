using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField] FiniteState firstState; // state to immediately begin on start
    FiniteState currentState; // stores the active state

    private void Start()
    {
        ChangeToState(firstState);
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    public void ChangeToState(FiniteState _nextState)
    {
        if(currentState != null)
            currentState.OnExit();
        currentState = _nextState;
        currentState.OnEnter(this);
    }
}
