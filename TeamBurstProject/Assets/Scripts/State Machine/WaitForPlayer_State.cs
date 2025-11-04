using UnityEngine;

public class WaitForPlayer_State : FiniteState
{
    [SerializeField] FiniteState nextState;
    [SerializeField] GameObject objectToRotate; // DELETE LATER. JUST FOR STATE MACHINE TESTING
    public override void OnUpdate()
    {
        base.OnUpdate();

        RotateObjectClockwise(); // DELETE LATER. JUST FOR STATE MACHINE TESTING
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameManager.instance.player)
        {
            fsMachine.ChangeToState(nextState);
        }
    }

    private void RotateObjectClockwise() // DELETE LATER. JUST FOR STATE MACHINE TESTING
    {
        objectToRotate.transform.localEulerAngles = new Vector3(0, objectToRotate.transform.eulerAngles.y + 20 * Time.deltaTime, 0); 
    }
}
