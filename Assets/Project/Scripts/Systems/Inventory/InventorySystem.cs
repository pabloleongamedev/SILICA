using System;

public class InventorySystem
{
    private InventoryGrid grid;

    public event Action<InventoryItemInstance> OnItemAdded;
    public event Action OnInventoryChanged;
    public event Action OnInventoryFull;

    public InventorySystem(int width, int height)
    {
        grid = new InventoryGrid(width, height);
    }

    // 🔥 NUEVO MÉTODO CORRECTO
    public bool AddItem(ItemData_SO itemData, int amount)
    {
        int remaining = amount;

        // 1. intentar stackear
        remaining = TryStackItem(itemData, remaining);

        // 2. crear nuevos stacks
        while (remaining > 0)
        {
            if (!grid.TryFindFirstEmptySlot(out int x, out int y))
            {
                OnInventoryFull?.Invoke();
                return false;
            }

            var instance = new InventoryItemInstance(itemData);

            int added = instance.Add(remaining);
            remaining -= added;

            grid.SetItem(x, y, instance);
            OnItemAdded?.Invoke(instance);
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    private int TryStackItem(ItemData_SO data, int amount)
    {
        int remaining = amount;

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (slot.IsEmpty)
                    continue;

                var item = slot.Item;

                // 🔥 COMPARACIÓN SEGURA POR ID
                if (item.Data.itemID != data.itemID || item.IsFull())
                    continue;

                int added = item.Add(remaining);
                remaining -= added;

                if (remaining <= 0)
                    return 0;
            }
        }

        return remaining;
    }

    public InventoryGrid GetGrid()
    {
        return grid;
    }

    public string GetDebugView()
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("=== INVENTORY ===");

        for (int y = grid.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (slot.IsEmpty)
                    sb.Append("[ EMPTY ]");
                else
                    sb.Append($"[ {slot.Item.Data.displayName} x{slot.Item.Quantity} ]");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public InventoryItemInstance SplitItem(int x, int y, int amount)
    {
        var slot = grid.GetSlot(x, y);

        if (slot.IsEmpty)
            return null;

        var item = slot.Item;

        if (amount >= item.Quantity)
            return null;

        item.Remove(amount);

        var newInstance = new InventoryItemInstance(item.Data);
        newInstance.Add(amount);

        OnInventoryChanged?.Invoke();

        return newInstance;
    }

    public bool MergeItems(int fromX, int fromY, int toX, int toY)
    {
        var fromSlot = grid.GetSlot(fromX, fromY);
        var toSlot = grid.GetSlot(toX, toY);

        if (fromSlot.IsEmpty || toSlot.IsEmpty)
            return false;

        var fromItem = fromSlot.Item;
        var toItem = toSlot.Item;

        // 🔥 COMPARACIÓN SEGURA
        if (fromItem.Data.itemID != toItem.Data.itemID)
            return false;

        int added = toItem.Add(fromItem.Quantity);
        fromItem.Remove(added);

        if (fromItem.IsEmpty())
            fromSlot.Clear();

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool MoveItem(int fromX, int fromY, int toX, int toY)
    {
        var fromSlot = grid.GetSlot(fromX, fromY);
        var toSlot = grid.GetSlot(toX, toY);

        if (fromSlot.IsEmpty)
            return false;

        if (toSlot.IsEmpty)
        {
            toSlot.SetItem(fromSlot.Item);
            fromSlot.Clear();

            OnInventoryChanged?.Invoke();
            return true;
        }

        // 🔥 COMPARACIÓN SEGURA
        if (fromSlot.Item.Data.itemID == toSlot.Item.Data.itemID)
        {
            return MergeItems(fromX, fromY, toX, toY);
        }

        var temp = toSlot.Item;
        toSlot.SetItem(fromSlot.Item);
        fromSlot.SetItem(temp);

        OnInventoryChanged?.Invoke();
        return true;
    }
}