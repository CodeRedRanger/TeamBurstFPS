using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundTrigger : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public AudioClip highlightSound; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayEffect(highlightSound, 1);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (highlightSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayEffect(highlightSound, 1);
        }
    }

}
