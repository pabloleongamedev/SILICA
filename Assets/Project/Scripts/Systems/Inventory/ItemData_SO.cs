using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Item")]
public class ItemData_SO : ScriptableObject
{
    public string itemID;
    public string displayName;
    public Sprite icon;

    [TextArea]
    public string description;

    // Para futuro crafting
    public int maxStack = 1;
    public int cantidad = 1;
}