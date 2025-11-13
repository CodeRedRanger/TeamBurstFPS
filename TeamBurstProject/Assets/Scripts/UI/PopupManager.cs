using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    [Header("Popups")]
    [SerializeField] PopupData[] popups;
    Dictionary<string, PopupData> popupDictionary;

    [Header("Centered Components")]
    [SerializeField] Image centeredPopup;
    [SerializeField] Image centeredShadow;
    [SerializeField] TMP_Text centeredText;

    [Header("Coner Components")]
    [SerializeField] Image cornerPopup;
    [SerializeField] Image cornerShadow;
    [SerializeField] TMP_Text cornerText;

    [Header("Colors")]
    [SerializeField] Color red;
    [SerializeField] Color redShadow;
    [SerializeField] Color blue, blueShadow;
    [SerializeField] Color green, greenShadow;
    [SerializeField] Color yellow, yellowShadow;
    [SerializeField] Color pink, pinkShadow;

    [Header("Other")]
    [SerializeField] Animator anim;



    private void Awake()
    {
        instance = this;

        popupDictionary = new Dictionary<string, PopupData>();
        foreach (PopupData popup in popups)
        {
            popupDictionary.Add(popup.name, popup);
        }
    }

    public void ShowPopup(string _name, float _duration = -1f)
    {
        StopAllCoroutines();
        if(_duration < 0f)
        {
            _duration = popupDictionary[_name].duration;
        }
        StartCoroutine(ShowPopupCoroutine(popupDictionary[_name], _duration));
    }

    public void HideAllPopups()
    {
        cornerPopup.gameObject.SetActive(false);
        centeredPopup.gameObject.SetActive(false);
    }

    private IEnumerator ShowPopupCoroutine(PopupData _data, float _duration)
    {
        anim.Play("ShowPopup");

        // COLORS

        Color _color = Color.white;
        Color _shadowColor = Color.white;

        switch (_data.color)
        {
            case PopupData.PopupColor.PINK:
                _color = pink;
                _shadowColor = pinkShadow;
                break;
            case PopupData.PopupColor.BLUE:
                _color = blue;
                _shadowColor = blueShadow;
                break;
            case PopupData.PopupColor.GREEN:
                _color = green;
                _shadowColor = greenShadow;
                break;
            case PopupData.PopupColor.RED:
                _color = red;
                _shadowColor = redShadow;
                break;
            case PopupData.PopupColor.YELLOW:
                _color = yellow;
                _shadowColor = yellowShadow;
                break;
        }


        // SET ACTIVE, SET COLORS, SET TEXT

        if (_data.type == PopupData.PopupType.CORNER)
        {  // CORNER POPUP

            // Set correct popup active
            cornerPopup.gameObject.SetActive(true);

            // Apply colors
            cornerPopup.color = _color;
            cornerShadow.color = _shadowColor;

            // set text
            cornerText.text = _data.text;
        }
        else
        { // CENTERED POPUP

            // Set correct popup active
            centeredPopup.gameObject.SetActive(true);

            // Apply colors
            centeredPopup.color = _color;
            centeredShadow.color = _shadowColor;

            // set text
            centeredText.text = _data.text;
        }


        // SOUND
        if (_data.sound != null)
            SoundManager.Instance.PlayEffect(_data.sound, 1);


        // WAIT AND REHIDE

        yield return new WaitForSeconds(_duration);
        HideAllPopups();
    }
}
