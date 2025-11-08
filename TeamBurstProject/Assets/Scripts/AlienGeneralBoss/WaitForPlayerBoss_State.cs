using UnityEngine;

public class WaitForPlayerBoss_State : FiniteState
{
    [SerializeField] FiniteState nextState;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCurrentState) return;

        if (other.CompareTag("Player"))
        {
            fsMachine.ChangeToState(nextState);
        }
    }
}
