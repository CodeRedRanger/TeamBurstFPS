using UnityEngine;

public class PlayerHotbar : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public int selectedSlotIndex = 0;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inventoryManager.hotbarSize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i-1);
            }
        }
    }

    void SelectSlot(int index)
    {
        //if (index >= 0 && index < inventoryManager.hotbarSize)
        {
            selectedSlotIndex = index;
            // Optionally, you can add visual feedback for the selected slot here
            Debug.Log("Selected hotbar slot: " + inventoryManager.hotbarItems[selectedSlotIndex]?.type);
        }
    }
}
