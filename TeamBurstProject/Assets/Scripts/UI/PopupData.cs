using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Popup Data")]
public class PopupData : ScriptableObject
{   
    public enum PopupType { CENTERED, CORNER }
    public enum PopupColor { RED, GREEN, BLUE, YELLOW, PINK }
    public PopupType type;
    public PopupColor color;
    [TextArea] public string text;
    public float duration = 3.0f;

    [Header("Optional")]
    public AudioClip sound;
}
