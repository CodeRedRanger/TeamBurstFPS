using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    Interactable currentInteractable;

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable _interactable = other.GetComponent<Interactable>();

        if (_interactable != null)
        {
            SetCurrentInteractable(_interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable _interactable = other.GetComponent<Interactable>();

        if (_interactable != null && _interactable == currentInteractable)
        {
            SetCurrentInteractable(null);
        }
    }

    private void SetCurrentInteractable(Interactable _newInteractable)
    {
        if (currentInteractable != null) currentInteractable.ShowPrompt(false);
        if (_newInteractable != null)
        {
            currentInteractable = _newInteractable;
            currentInteractable.ShowPrompt(true);
        }
    }
}
