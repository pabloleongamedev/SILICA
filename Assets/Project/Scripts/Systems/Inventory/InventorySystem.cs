using System;

public class InventorySystem : IInventoryWriteModel
{
    private InventoryGrid grid;
    private InventoryOperations operations;

    private GridInventoryAdapter adapter;

    public IInventoryReadModel ReadModel { get; private set; }

    public InventorySystem(InventoryConfig_SO config, InventoryGrid grid)
    {
        this.grid = grid;

        operations = new InventoryOperations(grid);

        adapter = new GridInventoryAdapter(grid);
        ReadModel = adapter;
    }

    // =========================
    // ADD (RETURN REMAINING)
    // =========================
    public int AddItem(ItemData_SO data, int amount)
    {
        return operations.AddItem(data, amount, NotifySlotChanged);
    }

    // =========================
    // MOVE BY INDEX (UI ENTRY POINT)
    // =========================
    public void MoveItem(int fromIndex, int toIndex)
    {
        var from = IndexToGrid(fromIndex);
        var to = IndexToGrid(toIndex);

        var fromSlot = GetSlot(from.x, from.y);
        var toSlot = GetSlot(to.x, to.y);

        if (fromSlot.IsEmpty)
            return;

        // MERGE si mismo item
        if (!toSlot.IsEmpty && fromSlot.Item.Data.itemID == toSlot.Item.Data.itemID)
        {
            MergeItem(from.x, from.y, to.x, to.y);
        }
        else
        {
            MoveItem(from.x, from.y, to.x, to.y);
        }
    }

    // =========================
    // MOVE (GRID)
    // =========================
    public void MoveItem(int fromX, int fromY, int toX, int toY)
    {
        operations.Move(fromX, fromY, toX, toY, NotifySlotChanged);
    }

    // =========================
    // MERGE (GRID)
    // =========================
    public void MergeItem(int fromX, int fromY, int toX, int toY)
    {
        operations.Merge(fromX, fromY, toX, toY, NotifySlotChanged);
    }

    // =========================
    // ACCESS
    // =========================
    public InventorySlot GetSlot(int x, int y)
    {
        return grid.GetSlot(x, y);
    }

    public (int x, int y) IndexToGrid(int index)
    {
        int x = index % grid.Width;
        int y = index / grid.Width;
        return (x, y);
    }

    public int GetAmount(ItemData_SO item)
    {
        int total = 0;

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = GetSlot(x, y); // 👈 TU método real

                if (slot.IsEmpty)
                    continue;

                if (slot.Item.Data.itemID == item.itemID)
                {
                    total += slot.Item.Quantity;
                }
            }
        }

        return total;
    }
    public void RemoveItem(ItemData_SO item, int amount)
    {
        int remaining = amount;

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (remaining <= 0)
                    return;

                var slot = GetSlot(x, y);

                if (slot.IsEmpty)
                    continue;

                if (slot.Item.Data.itemID != item.itemID)
                    continue;

                int removed = slot.Item.Remove(remaining);
                remaining -= removed;

                // 🔥 NOTIFICAR CAMBIO (CLAVE)
                NotifySlotChanged(x, y, slot.Item);

                // 🔥 si quedó vacío, notifícalo como null
                if (slot.Item.IsEmpty())
                {
                    NotifySlotChanged(x, y, null);
                }
            }
        }
    }
    // =========================
    // EVENT BRIDGE
    // =========================
    private void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        adapter.NotifySlotChanged(x, y, item);
    }
}