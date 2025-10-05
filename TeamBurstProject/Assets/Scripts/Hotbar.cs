using UnityEngine;

struct HotbarSlot
{
    public ItemType item;
    public int amount;
}

public class Hotbar : MonoBehaviour
{
    HotbarSlot[] slots = new HotbarSlot[5];

    public bool Add(ItemType item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            HotbarSlot currSlot = slots[i];
            if(currSlot.item == ItemType.none)
            {
                currSlot.item = item;
                currSlot.amount += amount;
                //update UI
                Debug.Log("added " + item + " to hotbar");
                return true;
            }
        } 
        return false;
    }

    public void Remove()
    {

    }
}
