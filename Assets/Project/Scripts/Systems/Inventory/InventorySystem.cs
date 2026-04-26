using UnityEngine;
using System.Collections.Generic;

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
        adapter = new GridInventoryAdapter(grid, this);

        ReadModel = adapter;
    }

    // =========================================================
    // CORE: SIMULACIÓN SINGLE
    // =========================================================
    private int CalculateAddableAmount(ItemData_SO item, int amount)
    {
        int canAdd = 0;

        // STACK
        if (item.maxStack > 1)
        {
            foreach (var slot in grid.GetAllSlots())
            {
                if (slot.IsEmpty)
                    continue;

                if (slot.ItemInstance.Data == item && !slot.ItemInstance.IsFull())
                {
                    canAdd += slot.ItemInstance.GetRemainingSpace();
                }
            }
        }

        // EMPTY
        if (canAdd < amount)
        {
            foreach (var slot in grid.GetAllSlots())
            {
                if (slot.IsEmpty)
                {
                    canAdd += item.maxStack;
                }

                if (canAdd >= amount)
                    break;
            }
        }

        return canAdd;
    }

    // =========================================================
    //  NUEVO: BATCH SIMULATION REAL
    // =========================================================
    public bool CanAddItemsBatch(params (ItemData_SO item, int amount)[] items)
    {
        var tempGrid = CloneGrid();
        var tempOps = new InventoryOperations(tempGrid);

        foreach (var entry in items)
        {
            int added = tempOps.AddItem(entry.item, entry.amount, null);

            if (added < entry.amount)
                return false;
        }

        return true;
    }

    // =========================================================
    // BATCH REAL: REMOVE + ADD (CRAFTING)
    // =========================================================
    public bool CanProcessBatch(
        (ItemData_SO item, int amount)[] remove,
        (ItemData_SO item, int amount)[] add)
    {
        // 1. CLONAR GRID
        var tempGrid = CloneGrid();
        var tempOps = new InventoryOperations(tempGrid);

        // 2. REMOVER PRIMERO (SIMULADO)
        foreach (var r in remove)
        {
            int remaining = r.amount;

            foreach (var slot in tempGrid.GetAllSlots())
            {
                if (remaining <= 0)
                    break;

                if (slot.IsEmpty)
                    continue;

                if (slot.ItemInstance.Data != r.item)
                    continue;

                int removed = slot.ItemInstance.Remove(remaining);
                remaining -= removed;

                if (slot.ItemInstance.IsEmpty())
                    slot.Clear();
            }

            // ❌ NO ALCANZA
            if (remaining > 0)
                return false;
        }

        // 3. INTENTAR INSERTAR
        foreach (var a in add)
        {
            int added = tempOps.AddItem(a.item, a.amount, null);

            if (added < a.amount)
                return false;
        }

        return true;
    }

    // =========================================================
    //  CLONACIÓN (CRÍTICO)
    // =========================================================
    private InventoryGrid CloneGrid()
    {
        var newGrid = new InventoryGrid(grid.Width, grid.Height);

        foreach (var slot in grid.GetAllSlots())
        {
            if (slot.IsEmpty)
                continue;

            var newInstance = new InventoryItemInstance(slot.ItemInstance.Data);
            newInstance.Add(slot.ItemInstance.Quantity);

            newGrid.GetSlot(slot.X, slot.Y).SetItem(newInstance);
        }

        return newGrid;
    }

    // =========================================================
    // ADD
    // =========================================================
    public int AddItem(ItemData_SO item, int amount)
    {
        int canAdd = CalculateAddableAmount(item, amount);

        if (canAdd <= 0)
        {
            Debug.LogWarning($"[Inventory] Cannot add {amount} of {item.name} - not enough space.");
            return 0;
        }

        // 🔥 BLOQUEO TOTAL (regla correcta)
        if (canAdd < amount)
        {
            Debug.LogWarning($"[Inventory] Partial insert blocked for {item.name}");
            return 0;
        }

        return operations.AddItem(item, amount, NotifySlotChanged);
    }

    // =========================================================
    // MOVE
    // =========================================================
    public void MoveItem(int fromIndex, int toIndex)
    {
        var from = IndexToGrid(fromIndex);
        var to = IndexToGrid(toIndex);

        var fromSlot = grid.GetSlot(from.x, from.y);
        var toSlot = grid.GetSlot(to.x, to.y);

        if (fromSlot.IsEmpty)
            return;

        if (!toSlot.IsEmpty && fromSlot.ItemInstance.Data == toSlot.ItemInstance.Data)
        {
            operations.Merge(from.x, from.y, to.x, to.y, NotifySlotChanged);
        }
        else
        {
            operations.Move(from.x, from.y, to.x, to.y, NotifySlotChanged);
        }
    }

    public void MoveItem(int fromX, int fromY, int toX, int toY)
    {
        operations.Move(fromX, fromY, toX, toY, NotifySlotChanged);
    }

    public void MergeItem(int fromX, int fromY, int toX, int toY)
    {
        operations.Merge(fromX, fromY, toX, toY, NotifySlotChanged);
    }

    // =========================================================
    // ACCESS
    // =========================================================
    public InventorySlot GetSlot(int x, int y)
    {
        return grid.GetSlot(x, y);
    }

    public (int x, int y) IndexToGrid(int index)
    {
        return (index % grid.Width, index / grid.Width);
    }

    public int GetAmount(ItemData_SO item)
    {
        int total = 0;

        foreach (var slot in grid.GetAllSlots())
        {
            if (slot.IsEmpty)
                continue;

            if (slot.ItemInstance.Data == item)
            {
                total += slot.ItemInstance.Quantity;
            }
        }

        return total;
    }

    public void RemoveItem(ItemData_SO item, int amount)
    {
        int remaining = amount;

        foreach (var slot in grid.GetAllSlots())
        {
            if (remaining <= 0)
                return;

            if (slot.IsEmpty)
                continue;

            if (slot.ItemInstance.Data != item)
                continue;

            int removed = slot.ItemInstance.Remove(remaining);
            remaining -= removed;

            if (slot.ItemInstance.IsEmpty())
            {
                slot.Clear();
                NotifySlotChanged(slot.X, slot.Y, null);
            }
            else
            {
                NotifySlotChanged(slot.X, slot.Y, slot.ItemInstance);
            }
        }
    }

    public void Clear()
    {
        foreach (var slot in grid.GetAllSlots())
        {
            if (!slot.IsEmpty)
            {
                slot.Clear();
                NotifySlotChanged(slot.X, slot.Y, null);
            }
        }
    }

    private void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        adapter.NotifySlotChanged(x, y, item);
    }
}