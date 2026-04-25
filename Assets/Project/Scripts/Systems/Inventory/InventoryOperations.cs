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
    public int AddItem(ItemData_SO item, int amount, Action<int, int, InventoryItemInstance> onChanged)
    {
        int remaining = amount;

        // =========================
        // STACK
        // =========================
        foreach (var slot in grid.GetAllSlots())
        {
            if (remaining <= 0) break;

            if (!slot.IsEmpty && slot.ItemInstance.Data == item && !slot.ItemInstance.IsFull())
            {
                int before = slot.ItemInstance.Quantity;

                int added = slot.ItemInstance.Add(remaining);
                remaining -= added;

                // 🔥 SOLO SI CAMBIÓ
                if (added > 0)
                {
                    onChanged?.Invoke(slot.X, slot.Y, slot.ItemInstance);
                }
            }
        }

        // =========================
        // EMPTY
        // =========================
        foreach (var slot in grid.GetAllSlots())
        {
            if (remaining <= 0) break;

            if (slot.IsEmpty)
            {
                var instance = new InventoryItemInstance(item);

                int added = instance.Add(remaining);
                if (added <= 0) continue;

                slot.SetItem(instance);
                remaining -= added;

                // 🔥 SOLO SI REALMENTE SE INSERTÓ
                onChanged?.Invoke(slot.X, slot.Y, slot.ItemInstance);
            }
        }

        return amount - remaining;
    }

    // =========================
    // MOVE
    // =========================
    public void Move(int fromX, int fromY, int toX, int toY, Action<int,int,InventoryItemInstance> onChanged)
    {
        var fromSlot = grid.GetSlot(fromX, fromY);
        var toSlot = grid.GetSlot(toX, toY);

        // 🔥 NO HACER NADA SI ES EL MISMO SLOT
        if (fromX == toX && fromY == toY)
            return;

        var fromItem = fromSlot.ItemInstance;
        var toItem = toSlot.ItemInstance;

        // 🔥 SI SON IGUALES (misma referencia), no hay cambio
        if (fromItem == toItem)
            return;

        fromSlot.SetItem(toItem);
        toSlot.SetItem(fromItem);

        onChanged?.Invoke(fromX, fromY, fromSlot.ItemInstance);
        onChanged?.Invoke(toX, toY, toSlot.ItemInstance);
    }

    // =========================
    // MERGE
    // =========================
    public void Merge(int fromX, int fromY, int toX, int toY, Action<int,int,InventoryItemInstance> onChanged)
    {
        var fromSlot = grid.GetSlot(fromX, fromY);
        var toSlot = grid.GetSlot(toX, toY);

        if (fromSlot.IsEmpty || toSlot.IsEmpty) return;

        var fromItem = fromSlot.ItemInstance;
        var toItem = toSlot.ItemInstance;

        if (fromItem.Data != toItem.Data) return;
        if (toItem.IsFull()) return;

        int beforeTo = toItem.Quantity;
        int beforeFrom = fromItem.Quantity;

        int moved = toItem.Add(fromItem.Quantity);
        if (moved <= 0) return;

        fromItem.Remove(moved);

        // 🔥 SOLO SI CAMBIÓ
        if (toItem.Quantity != beforeTo)
        {
            onChanged?.Invoke(toX, toY, toSlot.ItemInstance);
        }

        if (fromItem.IsEmpty())
        {
            fromSlot.Clear();
            onChanged?.Invoke(fromX, fromY, null);
        }
        else if (fromItem.Quantity != beforeFrom)
        {
            onChanged?.Invoke(fromX, fromY, fromSlot.ItemInstance);
        }
    }
}