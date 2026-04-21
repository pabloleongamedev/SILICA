using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryConfig_SO config;
    private InventorySystem inventory;
    

    private void Awake()
    {
        if (config == null)
        {
            Debug.LogWarning("[InventoryController] No config assigned, using default 5x5 inventory");
            inventory = new InventorySystem(5, 5);
        }
        else
        {
            inventory = new InventorySystem(config.width, config.height);
        }
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