using UnityEngine;
using UnityEngine.UI; 

public class HotbarSlot : MonoBehaviour
{
   public Image icon;

   //public Text amountText;

    public void UpdateSlot(ItemData item)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        //amountText.text = "";
    }

}
