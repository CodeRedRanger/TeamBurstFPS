using UnityEngine;


public enum ItemType
{
    bomb,
    grenade,
    stunner,
    medkit
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    public ItemType type;
    public int maxStack;
}
