using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Item")]
public class ItemData_SO : ScriptableObject
{
    public string itemID;
    public string displayName;
    public Sprite icon;
    public int maxStack = 99;

    [TextArea]
    public string description;

}