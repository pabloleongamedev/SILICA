using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private InventoryConfig_SO config;
    [Header("View")]
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private InventoryListView listView;
    [SerializeField] private UIButtonDropSlot CraftingTarget;
    

    private InventorySystem inventorySystem;
    private InventoryGrid grid;

    private void Awake()
    {
        grid = new InventoryGrid(config.width, config.height);
        inventorySystem = new InventorySystem(config, grid);

        if (inventoryView == null)
        {
            Debug.LogError("InventoryView not assigned");
            return;
        }

        // 🔥 INICIALIZACIÓN
        inventoryView.Initialize(inventorySystem.ReadModel);
        listView.Initialize(inventorySystem.ReadModel); 

        // 🔥 CONEXIÓN EVENTO (CLAVE)
        inventoryView.OnItemDropped += MoveItem;
        CraftingTarget.OnItemDropped += HandleItemUsed;
        
        // 🔥 conectar drag
        listView.OnItemDropped += MoveItem; 
    }

    // =========================
    // ADD ITEM
    // =========================
    public int TryAddItem(ItemData_SO data, int amount)
    {
        return inventorySystem.AddItem(data, amount);
    }
    private void HandleItemUsed(InventoryItemInstance item)
    {
        Debug.Log("Usando item: " + item.Data.displayName);

        // ejemplo:
        item.Remove(1);

        // 🔥 refrescar UI (si no tienes eventos automáticos)
        inventorySystem.AddItem(item.Data, 0); 
    }

    // =========================
    // MOVE / MERGE
    // =========================
    public void MoveItem(int fromIndex, int toIndex)
    {
        var from = inventorySystem.IndexToGrid(fromIndex);
        var to = inventorySystem.IndexToGrid(toIndex);

        var fromSlot = inventorySystem.GetSlot(from.x, from.y);
        var toSlot = inventorySystem.GetSlot(to.x, to.y);

        if (fromSlot.IsEmpty)
            return;

        // 🔥 DECISIÓN: MERGE o SWAP
        if (!toSlot.IsEmpty && fromSlot.Item.Data.itemID == toSlot.Item.Data.itemID)
        {
            inventorySystem.MergeItem(from.x, from.y, to.x, to.y);
        }
        else
        {
            inventorySystem.MoveItem(from.x, from.y, to.x, to.y);
        }
    }

    // =========================
    // ACCESS
    // =========================
    public InventorySystem GetInventorySystem()
    {
        return inventorySystem;
    }
}