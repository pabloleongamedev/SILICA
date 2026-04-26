using System;

public class GridInventoryAdapter : IInventoryReadModel
{
    private InventoryGrid grid;
    private InventorySystem system;

    public int Capacity => grid.Width * grid.Height;

    public event Action<int, InventoryItemInstance> OnItemChanged;

    public GridInventoryAdapter(InventoryGrid grid, InventorySystem system)
    {
        this.grid = grid;
        this.system = system;
    }

    // =========================================================
    // GET ITEM
    // =========================================================
    public InventoryItemInstance GetItem(int index)
    {
        int x = index % grid.Width;
        int y = index / grid.Width;

        var slot = grid.GetSlot(x, y);
        return slot?.ItemInstance;
    }

    // =========================================================
    // GET AMOUNT
    // =========================================================
    public int GetAmount(ItemData_SO item)
    {
        int total = 0;

        foreach (var slot in grid.GetAllSlots())
        {
            if (slot.IsEmpty)
                continue;

            if (slot.ItemInstance.Data == item)
                total += slot.ItemInstance.Quantity;
        }

        return total;
    }

    // =========================================================
    // NOTIFY SLOT
    // =========================================================
    public void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        int index = y * grid.Width + x;
        OnItemChanged?.Invoke(index, item);
    }

    // =========================================================
    // VALIDATION (SIMPLE)
    // =========================================================
    public bool CanAddItem(ItemData_SO item, int amount)
    {
        return system.CanAddItemsBatch((item, amount));
    }

    // =========================================================
    // VALIDATION (BATCH)
    // =========================================================
    public bool CanAddItemsBatch(params (ItemData_SO item, int amount)[] items)
    {
        return system.CanAddItemsBatch(items);
    }

    // =========================================================
    // PROCESS VALIDATION
    // =========================================================
    public bool CanProcessBatch(
        (ItemData_SO item, int amount)[] remove,
        (ItemData_SO item, int amount)[] add)
    {
        return system.CanProcessBatch(remove, add);
    }
}