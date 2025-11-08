using UnityEngine;
using TMPro;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] public TMP_Text textUI;
    public abstract void Interact();
    public void ShowPrompt(bool _shouldShow)
    {
        textUI.enabled = _shouldShow;
    }
}
