using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowTurret_State : FiniteState
{
    [SerializeField] GameObject objectToRotate;
    [SerializeField] FiniteState shootState;
    [SerializeField] FiniteState targetLostState;
    [SerializeField] float durationBetweenFiring;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] float maxSightDistance;
    [SerializeField] float rotationSpeed;
    float fireTimer = 0;
    Vector3 directionToTarget;

    public override void OnUpdate()
    {
        base.OnUpdate();

        directionToTarget = gameManager.instance.player.transform.position - objectToRotate.transform.position;

        checkConditions();

        Rotate();
    }

    public void checkConditions()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= durationBetweenFiring)
            Fire();

        RaycastHit hit;
        Physics.Raycast(objectToRotate.transform.position, directionToTarget, out hit, maxSightDistance, ~ignoreLayers);
        if(hit.collider == null || hit.collider.gameObject != gameManager.instance.player)
        {
            fsMachine.ChangeToState(targetLostState);
        }
    }

    public void Fire()
    {
        fireTimer = 0;
        fsMachine.ChangeToState(shootState);
    }

    public override void OnEnter(FiniteStateMachine _calledByMachine)
    {
        base.OnEnter(_calledByMachine);
        fireTimer = 0;
    }

    void Rotate()
    {
        float _angleDifference = Quaternion.Angle(objectToRotate.transform.rotation, Quaternion.LookRotation(directionToTarget));
        objectToRotate.transform.rotation = Quaternion.Lerp(objectToRotate.transform.rotation, Quaternion.LookRotation(directionToTarget), rotationSpeed * Time.deltaTime / _angleDifference);
        
    }

}
