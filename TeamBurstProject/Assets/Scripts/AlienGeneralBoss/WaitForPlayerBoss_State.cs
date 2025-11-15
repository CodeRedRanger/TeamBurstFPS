using UnityEngine;
using UnityEngine.UI;

public class WaitForPlayerBoss_State : FiniteState
{
    [SerializeField] FiniteState nextState;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCurrentState) return;

        if (other.CompareTag("Player"))
        {
            gameManager.instance.bossHPBar.transform.parent.gameObject.SetActive(true);
            gameManager.instance.bossHPBar.GetComponent<Image>().fillAmount = 1;
            fsMachine.ChangeToState(nextState);
        }
    }
}
