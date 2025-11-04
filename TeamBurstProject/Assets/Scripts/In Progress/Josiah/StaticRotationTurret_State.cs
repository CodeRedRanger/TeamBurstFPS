using UnityEngine;

public class StaticRotationTurret_State : FiniteState
{
    [SerializeField] FiniteState nextState;
    [SerializeField] GameObject objectToRotate; // DELETE LATER. JUST FOR STATE MACHINE TESTING

    public override void OnUpdate()
    {
        base.OnUpdate();

        RotateObjectClockwise(); // DELETE LATER. JUST FOR STATE MACHINE TESTING

        if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) >= 12)
            fsMachine.ChangeToState(nextState);
    }

    private void RotateObjectClockwise() // DELETE LATER. JUST FOR STATE MACHINE TESTING
    {
        objectToRotate.transform.localEulerAngles = new Vector3(objectToRotate.transform.eulerAngles.x + 20 * Time.deltaTime, 0, 0);
    }
}
