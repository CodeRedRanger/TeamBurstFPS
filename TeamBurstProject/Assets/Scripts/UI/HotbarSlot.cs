using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class HotbarSlot : MonoBehaviour
{
   public Image icon;

   //public Text amountText;
   public TextMeshProUGUI buttonInput;

    public void UpdateSlot(ItemData item)
    {
        icon.sprite = item.icon;
        icon.enabled = true;

        buttonInput = icon.GetComponentInChildren<TextMeshProUGUI>();

        buttonInput.text = item.keyPress;
        
        
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        //amountText.text = "";
    }

}
