using UnityEngine;

public enum Items
{
    none,
    medkit,
    grenade
}

struct HotbarSlot
{
    public Items item;
    public int amount;
}

public class Hotbar : MonoBehaviour
{
    HotbarSlot[] slots = new HotbarSlot[5];

    public bool Add(Items item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            HotbarSlot currSlot = slots[i];
            if(currSlot.item == Items.none)
            {
                currSlot.item = item;
                currSlot.amount += amount;
                //update UI
                print("added " + item + " to hotbar");
                return true;
            }
        } 
        return false;
    }

    public void Remove()
    {

    }
}
