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

        if (canAdd < amount)
        {
            foreach (var slot in grid.GetAllSlots())
            {
                if (slot.IsEmpty)
                    canAdd += item.maxStack;

                if (canAdd >= amount)
                    break;
            }
        }

        return canAdd;
    }

    // =========================================================
    // BATCH SIMULATION
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

    public bool CanProcessBatch(
        (ItemData_SO item, int amount)[] remove,
        (ItemData_SO item, int amount)[] add)
    {
        var tempGrid = CloneGrid();
        var tempOps = new InventoryOperations(tempGrid);

        foreach (var r in remove)
        {
            int remaining = r.amount;

            foreach (var slot in tempGrid.GetAllSlots())
            {
                if (remaining <= 0)
                    break;

                if (slot.IsEmpty || slot.ItemInstance.Data != r.item)
                    continue;

                int removed = slot.ItemInstance.Remove(remaining);
                remaining -= removed;

                if (slot.ItemInstance.IsEmpty())
                    slot.Clear();
            }

            if (remaining > 0)
                return false;
        }

        foreach (var a in add)
        {
            int added = tempOps.AddItem(a.item, a.amount, null);

            if (added < a.amount)
                return false;
        }

        return true;
    }

    // =========================================================
    // CLONE
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
    // ADD (CON CONTROL DE NOTIFICACIÓN)
    // =========================================================
     //   IMPLEMENTACIÓN DE INTERFAZ (NO TOCAR)
    public int AddItem(ItemData_SO item, int amount)
    {
        return AddItem(item, amount, true);
    }
    public int AddItem(ItemData_SO item, int amount, bool notify = true)
    {
        int canAdd = CalculateAddableAmount(item, amount);

        if (canAdd <= 0)
        {
            if (notify)
                Notify($"Inventario lleno para {item.itemID}", NotificationType.Warning);
            return 0;
        }

        if (canAdd < amount)
        {
            if (notify)
                Notify($"No hay espacio suficiente para {item.itemID}", NotificationType.Warning);
            return 0;
        }

        int added = operations.AddItem(item, amount, NotifySlotChanged);

        if (added > 0 && notify)
        {
            Notify($"Has obtenido {item.itemID} x{added}", NotificationType.Success);
        }

        return added;
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

    // =========================================================
    // REMOVE
    // =========================================================
    public void RemoveItem(ItemData_SO item, int amount)
    {
        int remaining = amount;
        int removedTotal = 0;

        foreach (var slot in grid.GetAllSlots())
        {
            if (remaining <= 0)
                break;

            if (slot.IsEmpty || slot.ItemInstance.Data != item)
                continue;

            int removed = slot.ItemInstance.Remove(remaining);
            remaining -= removed;
            removedTotal += removed;

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

        if (removedTotal > 0)
        {
            Notify($"{item.itemID} x{removedTotal} consumido", NotificationType.Info);
        }
    }

    // =========================================================
    // CLEAR
    // =========================================================
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

        Notify("Inventario limpiado", NotificationType.Info);
    }

    // =========================================================
    // ACCESS
    // =========================================================
    public InventorySlot GetSlot(int x, int y) => grid.GetSlot(x, y);

    public (int x, int y) IndexToGrid(int index)
        => (index % grid.Width, index / grid.Width);

    public int GetAmount(ItemData_SO item)
    {
        int total = 0;

        foreach (var slot in grid.GetAllSlots())
        {
            if (!slot.IsEmpty && slot.ItemInstance.Data == item)
                total += slot.ItemInstance.Quantity;
        }

        return total;
    }

    // =========================================================
    // NOTIFY
    // =========================================================
    private void Notify(string message, NotificationType type)
    {
        Debug.Log("[Inventory] " + message);

        GameplayEvents.OnNotification?.Invoke(new NotificationData
        {
            message = message,
            type = type
        });
    }

    private void NotifySlotChanged(int x, int y, InventoryItemInstance item)
    {
        adapter.NotifySlotChanged(x, y, item);
    }
    // =========================================================
    // MOVE (GRID COORDS)
    // =========================================================
    public void MoveItem(int fromX, int fromY, int toX, int toY)
    {
        operations.Move(fromX, fromY, toX, toY, NotifySlotChanged);
    }

    // =========================================================
    // MERGE
    // =========================================================
    public void MergeItem(int fromX, int fromY, int toX, int toY)
    {
        operations.Merge(fromX, fromY, toX, toY, NotifySlotChanged);
    }
}