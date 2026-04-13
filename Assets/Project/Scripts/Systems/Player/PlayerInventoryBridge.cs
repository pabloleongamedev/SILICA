using UnityEngine;

public class PlayerInventoryBridge : MonoBehaviour
{
    private InventorySystem inventory;

    private void Awake()
    {
        inventory = new InventorySystem(5, 4);
    }

    public bool TryAddItem(ItemData_SO itemData)
    {
        return inventory.AddItem(itemData);
    }

    public InventorySystem GetInventory()
    {
        return inventory;
    }
}