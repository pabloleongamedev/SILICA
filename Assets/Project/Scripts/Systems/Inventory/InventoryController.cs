using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryConfig_SO config;
    private InventorySystem inventory;
    

    private void Awake()
    {
        inventory = new InventorySystem(config.width, config.height);
    }

    public bool TryAddItem(ItemData_SO itemData, int quantity)
    {
        return inventory.AddItem(itemData, quantity);
    }

    public InventorySystem GetInventory()
    {
        return inventory;
    }
}