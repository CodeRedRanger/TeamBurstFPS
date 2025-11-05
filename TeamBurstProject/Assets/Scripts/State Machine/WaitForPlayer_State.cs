using UnityEngine;

public class WaitForPlayer_State : FiniteState
{
    [SerializeField] FiniteState nextState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameManager.instance.player)
        {
            fsMachine.ChangeToState(nextState);
        }
    }
}
