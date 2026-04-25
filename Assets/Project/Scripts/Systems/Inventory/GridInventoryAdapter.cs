using System;

public class GridInventoryAdapter : IInventoryReadModel
{
    private InventoryGrid grid;

    public int Capacity => grid.Width * grid.Height;

    public event Action<int, InventoryItemInstance> OnItemChanged;

    public GridInventoryAdapter(InventoryGrid grid)
    {
        this.grid = grid;
    }

    public InventoryItemInstance GetItem(int index)
    {
        int x = index % grid.Width;
        int y = index / grid.Width;

        var slot = grid.GetSlot(x, y);
        return slot?.ItemInstance;
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
}