using UnityEngine;


public enum ItemType
{
    medkit,
    grenade,
    bomb
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    public ItemType type;
    public int maxStack;
}
