using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private InventoryConfig_SO config;
    [Header("View")]
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private InventoryListView listView;

public IInventoryReadModel ReadModel => inventorySystem.ReadModel;
public IInventoryWriteModel WriteModel => inventorySystem;  

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
    }
    private void Start()
    {
        inventoryView.Initialize(inventorySystem.ReadModel);
        listView.Initialize(inventorySystem.ReadModel);

        inventoryView.OnItemDropped += MoveItem;
        listView.OnItemDropped += MoveItem;

        // 🔥 CLAVE: sincronizar después de todo
        inventoryView.ForceRefresh();
    }


    // =========================
    // ADD ITEM
    // =========================
    public int TryAddItem(ItemData_SO data, int amount)
    {
        return inventorySystem.AddItem(data, amount);
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
        if (!toSlot.IsEmpty && fromSlot.ItemInstance.Data.itemID == toSlot.ItemInstance.Data.itemID)
        {
            inventorySystem.MergeItem(from.x, from.y, to.x, to.y);
        }
        else
        {
            inventorySystem.MoveItem(from.x, from.y, to.x, to.y);
        }
    }
    public void ResetInventory()
    {
        inventorySystem.Clear();

        // opcional pero recomendado para seguridad visual
        inventoryView.ForceRefresh();
    }

    // =========================
    // ACCESS
    // =========================
    public InventorySystem GetInventorySystem()
    {
        return inventorySystem;
    }
}