using UnityEngine;

public class WaitForPlayer_State : FiniteState
{
    [SerializeField] FiniteState nextState;
    [SerializeField] Transform lineOfSightPos;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] float maxSightDistance;

    private void OnTriggerStay(Collider other)
    {
        if (!isCurrentState) return;

        if (other.gameObject == gameManager.instance.player)
        {
            RaycastHit hit;
            Physics.Raycast(lineOfSightPos.position, gameManager.instance.player.transform.position - lineOfSightPos.position, out hit, maxSightDistance, ~ignoreLayers);
            if (hit.collider != null && hit.collider.gameObject == gameManager.instance.player)
            {
                fsMachine.ChangeToState(nextState);
            }
        }
    }
}
