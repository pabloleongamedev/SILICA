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

    public InventoryItemInstance GetItem(int index)
    {
        int x = index % grid.Width;
        int y = index / grid.Width;

        var slot = grid.GetSlot(x, y);
        return slot?.ItemInstance;
    }
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

    public void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        int index = y * grid.Width + x;
        OnItemChanged?.Invoke(index, item);
    }

    // IMPLEMENTACIÓN CORRECTA DE LA INTERFAZ
    public bool CanAddItem(ItemData_SO item, int amount)
    {
        int remaining = amount;

        // 1. LLENAR STACKS EXISTENTES
        foreach (var slot in grid.GetAllSlots())
        {
            if (slot.IsEmpty)
                continue;

            if (slot.ItemData != item)
                continue;

            if (!slot.HasSpace)
                continue;

            int space = slot.RemainingSpace;
            remaining -= space;

            if (remaining <= 0)
                return true;
        }

        // 2. USAR SLOTS VACÍOS
        foreach (var slot in grid.GetAllSlots())
        {
            if (!slot.IsEmpty)
                continue;

            remaining -= item.maxStack;

            if (remaining <= 0)
                return true;
        }

        // ❌ NO CABE TODO
        return false;
    }
    public bool CanAddItemsBatch(params (ItemData_SO item, int amount)[] items)
    {
        // 🔥 simulación REAL sobre copia del grid
        var simulationGrid = grid.Clone(); // necesitas esto

        var operations = new InventoryOperations(simulationGrid);

        foreach (var (item, amount) in items)
        {
            int added = operations.AddItem(item, amount, null);

            if (added < amount)
                return false;
        }

        return true;
    }
    public bool CanProcessBatch((ItemData_SO item, int amount)[] remove,(ItemData_SO item, int amount)[] add)
    {
        return system.CanProcessBatch(remove, add);
    }
}