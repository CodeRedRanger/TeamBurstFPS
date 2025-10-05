using UnityEngine;

enum ItemType
{
    none,
    medkit,
    grenade
}

[CreateAssetMenu(menuName = "Items/Item Type")]
public class ItemData : ScriptableObject
{
    // image/icon
    ItemType type;
    public int maxStack;
}
