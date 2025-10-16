using UnityEngine;
using UnityEngine.UI;

public class HotbarRobb : MonoBehaviour
{
    
    /*
    struct HotbarSlot
    {
        public ItemData item;
        public int amount;
    }

    public class Hotbar : MonoBehaviour
    {
        [SerializeField] RectTransform hotbarPanel;
        HotbarSlot[] slots = new HotbarSlot[5];

        public bool Add(ItemData item, int amount)
        {
            //searches for slot containing same item
            for (int i = 0; i < slots.Length; i++)
            {
                //if the slot hasnt reached max amount then add
                if (slots[i].item == item && slots[i].amount < item.maxStack)
                {
                    slots[i].item = item;
                    slots[i].amount += amount;
                    UpdateUI();
                    //Debug.Log("added " + item + " to hotbar");
                    Debug.Log("slot " + (i + 1).ToString() + " has: " + slots[i].amount + " items");
                    //Debug.Log(slots[4].amount);
                    return true;
                }
            }
            //searches for any empty slot
            for (int i = 0; i < slots.Length; i++)
            {
                //if empty slot found then add
                if (!slots[i].item)
                {
                    slots[i].item = item;
                    slots[i].amount += amount;
                    UpdateUI();
                    //Debug.Log("added " + item + " to hotbar");
                    Debug.Log("slot " + (i + 1).ToString() + " has: " + slots[i].amount + " items");
                    //Debug.Log(slots[4].amount);
                    return true;
                }
            }
            Debug.Log("hotbar full");
            return false;
        }

        public void Remove()
        {

        }

        void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                Transform currSlotUI = hotbarPanel.GetChild(i);
                Transform icon = currSlotUI.Find("Icon");

                Image image = icon.GetComponent<Image>();

                if (slots[i].item)
                {
                    image.sprite = slots[i].item.icon;
                    image.gameObject.SetActive(true);
                    // change number under slot UI to slot[i].amount
                    // enable number under slot UI
                }
                else
                {
                    image.sprite = null;
                    image.gameObject.SetActive(false);
                    // disable number under slot UI
                    // change number under slot UI to 1
                }
            }
        }



    }*/

} 
