using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] UnityEvent onEvent;
    [SerializeField] UnityEvent offEvent;
    [SerializeField] GameObject[] toLightUp;
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

            if (toLightUp != null)
            {
                for (int i = 0; i < toLightUp.Length; i++)
                {
                    Renderer rend = toLightUp[i].GetComponent<Renderer>();
                    rend.material.SetColor("_EmissionColor", rend.material.GetColor("_EmissionColor") * 35);
                }
            }
        }
        else
        {
            offEvent.Invoke();
            anim.Play(offAnimation);

            if (toLightUp != null)
            {
                for (int i = 0; i < toLightUp.Length; i++)
                {
                    Renderer rend = toLightUp[i].GetComponent<Renderer>();
                    rend.material.SetColor("_EmissionColor", rend.material.GetColor("_EmissionColor") / 35);
                }
            }
        }
    }
}
