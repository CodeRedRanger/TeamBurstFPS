using UnityEngine;

public class SpawnEnemy_State : FiniteState
{
    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {

    }
}
