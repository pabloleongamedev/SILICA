using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData_SO: ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [Header("Grid Size")]
    public int width = 1;
    public int height = 1;

    [Header("Stack")]
    public bool stackable = false;
    public int maxStack = 1;
}