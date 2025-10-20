using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; 


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> hotbarItems = new List<ItemData>();
    public int hotbarSize = 3;

    //Assign these in the Inspector of InventoryManager 
    public GameObject[] hotbarSlots; // = new GameObject[3]; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance != null)
        {
            return; 
           
        }
        else
        {
            Instance = this;
            for (int i = 0; i < hotbarSize; i++)
            {
                hotbarItems.Add(null);
            }
        }
    }

    public void AddItem(ItemData item)
    {
        for (int i = 0; i < hotbarSize; i++)
        {
            
            if (hotbarItems[i] == null)
            {
                //Debug.Log("Added " + item.type + " to hotbar slot " + (i + 1));
                hotbarItems[i] = item;
                UpdateHotbarUI();
                return;
                

                //Check in here for the amount and if less than three add to the amount instead of adding a new item, if 0, add new item, if 3, do nothing
            }

            
        }
        
        //Debug.Log("Hotbar is full!");
        
    }

    public void UpdateHotbarUI()
    {


        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            var slot = hotbarSlots[i].GetComponent<HotbarSlot>();
            if (hotbarItems[i] != null)
            {
                slot.UpdateSlot(hotbarItems[i]);
            }
            else
            {
                //slot.ClearSlot();
            }
        }
    }
}
