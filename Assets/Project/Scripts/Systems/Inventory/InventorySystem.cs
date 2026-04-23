using System;

<<<<<<< HEAD
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

    public bool AddItem(ItemData_SO itemData)
    {
        int remaining = itemData.cantidad;

        // 🔥 1. intentar stackear primero
        remaining = TryStackItem(itemData, remaining);

        // 🔥 2. crear nuevos stacks si sobra
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

    public bool AddItem(ItemData_SO itemData, int amount)
    {
        int remaining = amount;

        // 🔥 1. intentar stackear primero
        remaining = TryStackItem(itemData, remaining);

        // 🔥 2. crear nuevos stacks si sobra
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

    private bool TryAddInstance(InventoryItemInstance item)
    {
        if (!grid.TryFindFirstEmptySlot(out int x, out int y))
            return false;

        grid.SetItem(x, y, item);
        return true;
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
                    sb.Append($"[ {slot.Item.Data.displayName} ]");
            }

            sb.AppendLine();
        }

        return sb.ToString();
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

                // mismo tipo + stackable
                if (item.Data != data || item.IsFull())
                    continue;

                int added = item.Add(remaining);
                remaining -= added;

                if (remaining <= 0)
                    return 0;
            }
        }

        return remaining;
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

        if (fromItem.Data != toItem.Data)
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

        // si destino vacío → mover directo
        if (toSlot.IsEmpty)
        {
            toSlot.SetItem(fromSlot.Item);
            fromSlot.Clear();

            OnInventoryChanged?.Invoke();
            return true;
        }

        // si mismo tipo → merge
        if (fromSlot.Item.Data == toSlot.Item.Data)
        {
            return MergeItems(fromX, fromY, toX, toY);
        }

        // swap
        var temp = toSlot.Item;
        toSlot.SetItem(fromSlot.Item);
        fromSlot.SetItem(temp);

        OnInventoryChanged?.Invoke();
        return true;
    }

    public int GetAmount(ItemData_SO itemData)
=======
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
>>>>>>> 7ca46c4 (restore scripts interaction system)
    {
        int total = 0;

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
<<<<<<< HEAD
                var slot = grid.GetSlot(x, y);

                if (!slot.IsEmpty && slot.Item.Data == itemData)
=======
                var slot = GetSlot(x, y); // 👈 TU método real

                if (slot.IsEmpty)
                    continue;

                if (slot.Item.Data.itemID == item.itemID)
>>>>>>> 7ca46c4 (restore scripts interaction system)
                {
                    total += slot.Item.Quantity;
                }
            }
        }

        return total;
    }
<<<<<<< HEAD

    public bool RemoveItem(ItemData_SO itemData, int amount)
    {
        int remaining = amount;

        for (int y = 0; y < grid.Height && remaining > 0; y++)
        {
            for (int x = 0; x < grid.Width && remaining > 0; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (slot.IsEmpty || slot.Item.Data != itemData)
                    continue;

                int removed = System.Math.Min(remaining, slot.Item.Quantity);
                slot.Item.Remove(removed);
                remaining -= removed;

                if (slot.Item.IsEmpty())
                    slot.Clear();
            }
        }

        if (remaining <= 0)
        {
            OnInventoryChanged?.Invoke();
            return true;
        }

        return false;
    }

=======
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

                // 🔥 SI QUEDA VACÍO → LIMPIAR SLOT REAL
                if (slot.Item.IsEmpty())
                {
                    slot.Clear(); // 👈 ESTO ES LO QUE TE FALTA

                    NotifySlotChanged(x, y, null);
                }
                else
                {
                    NotifySlotChanged(x, y, slot.Item);
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
>>>>>>> 7ca46c4 (restore scripts interaction system)
}