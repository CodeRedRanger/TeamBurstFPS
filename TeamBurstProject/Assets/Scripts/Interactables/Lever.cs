using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] UnityEvent onEvent;
    [SerializeField] UnityEvent offEvent;
    [SerializeField] string onAnimation;
    [SerializeField] string offAnimation;
    [SerializeField] Animator anim;
    bool isOn;

    public override void Interact()
    {
        isOn = !isOn;
        if (isOn)
        {
            onEvent.Invoke();
            anim.Play(onAnimation);
        }
        else
        {
            offEvent.Invoke();
            anim.Play(offAnimation);
        }
    }
}
