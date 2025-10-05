using UnityEngine;

public enum ItemType
{
    none,
    medkit,
    grenade
}

//public struct ItemData
//{
//    // image/icon
//    public Items item;
//    public int maxStack;
//} 

[CreateAssetMenu(menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    // image/icon
    public ItemType item;
    public int maxStack;
}
