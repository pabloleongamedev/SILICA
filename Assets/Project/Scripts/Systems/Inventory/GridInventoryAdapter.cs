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
        return slot != null ? slot.Item : null;
    }

    public void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        int index = y * grid.Width + x;
        OnItemChanged?.Invoke(index, item);
    }
}