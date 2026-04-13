using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryConfig_SO config;
    private InventorySystem inventory;
    

    private void Awake()
    {
        inventory = new InventorySystem(config.width, config.height);
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