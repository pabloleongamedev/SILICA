using System;

public class InventoryOperations
{
    private InventoryGrid grid;

    public InventoryOperations(InventoryGrid grid)
    {
        this.grid = grid;
    }

    // =========================
    // ADD
    // =========================
    public int AddItem(ItemData_SO data, int amount, Action<int,int,InventoryItemInstance> onChanged)
    {
        int remaining = amount;

        // STACK EXISTENTE
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (slot.IsEmpty)
                    continue;

                var item = slot.Item;

                if (item.Data.itemID != data.itemID || item.IsFull())
                    continue;

                int added = item.Add(remaining);
                remaining -= added;

                onChanged?.Invoke(x, y, item);

                if (remaining <= 0)
                    return 0;
            }
        }

        // ESPACIOS VACÍOS
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (!slot.IsEmpty)
                    continue;

                int stackSize = Math.Min(remaining, data.maxStack);

                var instance = new InventoryItemInstance(data);
                instance.Add(stackSize);

                slot.SetItem(instance);

                onChanged?.Invoke(x, y, instance);

                remaining -= stackSize;

                if (remaining <= 0)
                    return 0;
            }
        }

        // 🔥 DEVUELVE LO QUE SOBRÓ
        return remaining;
    }
    // =========================
    // MOVE
    // =========================
    public void Move(int fromX, int fromY, int toX, int toY, Action<int,int,InventoryItemInstance> onChanged)
    {
        var fromSlot = grid.GetSlot(fromX, fromY);
        var toSlot = grid.GetSlot(toX, toY);

        var fromItem = fromSlot.Item;
        var toItem = toSlot.Item;

        fromSlot.SetItem(toItem);
        toSlot.SetItem(fromItem);

        onChanged?.Invoke(fromX, fromY, fromSlot.Item);
        onChanged?.Invoke(toX, toY, toSlot.Item);
    }

    // =========================
    // MERGE
    // =========================
    public void Merge(int fromX, int fromY, int toX, int toY, Action<int,int,InventoryItemInstance> onChanged)
    {
        var fromSlot = grid.GetSlot(fromX, fromY);
        var toSlot = grid.GetSlot(toX, toY);

        if (fromSlot.IsEmpty || toSlot.IsEmpty)
            return;

        var fromItem = fromSlot.Item;
        var toItem = toSlot.Item;

        if (fromItem.Data.itemID != toItem.Data.itemID)
            return;

        if (toItem.IsFull())
            return;

        int moved = toItem.Add(fromItem.Quantity);
        fromItem.Remove(moved);

        if (fromItem.IsEmpty())
            fromSlot.Clear();

        onChanged?.Invoke(fromX, fromY, fromSlot.Item);
        onChanged?.Invoke(toX, toY, toSlot.Item);
    }
}