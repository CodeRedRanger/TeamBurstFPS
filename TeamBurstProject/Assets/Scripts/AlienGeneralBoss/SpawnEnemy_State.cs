using UnityEngine;

public class SpawnEnemy_State : FiniteState
{
    [SerializeField] Animator anim;
    [SerializeField] string animationToPlay;
    [SerializeField] FiniteState nextState;

    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        anim.Play(animationToPlay);
    }

    public void SpawnEnemies()
    {
        gameObject.GetComponentInChildren<BossSoldierSpawner>().StartSpawning();
    }

    public void MoveToNextState()
    {
        gameObject.GetComponentInChildren<BossSoldierSpawner>().ResetSpawning();
        fsMachine.ChangeToState(nextState);
    }
}
