using UnityEngine;

struct HotbarSlot
{
    public ItemData item;
    public int amount;
}

public class Hotbar : MonoBehaviour
{
    HotbarSlot[] slots = new HotbarSlot[5];

    public bool Add(ItemData item, int amount)
    {
        //searches for slot containing same item type
        for (int i = 0; i < slots.Length; i++)
        {
            //if the slot hasnt reached max amount then add
            if (slots[i].item == item && slots[i].amount < item.maxStack)
            {
                slots[i].item = item;
                slots[i].amount += amount;
                //update UI
                //Debug.Log("added " + item + " to hotbar");
                Debug.Log("slot " + (i+1).ToString() + " has: " + slots[i].amount + " items");
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
                //update UI
                //Debug.Log("added " + item + " to hotbar");
                Debug.Log("slot " + (i+1).ToString() + " has: " + slots[i].amount + " items");
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
}
