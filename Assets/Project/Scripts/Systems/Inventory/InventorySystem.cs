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

    // =========================
    // EVENT BRIDGE
    // =========================
    private void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        adapter.NotifySlotChanged(x, y, item);
    }
}