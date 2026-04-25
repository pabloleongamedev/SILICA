using System;

public interface IInventoryReadModel
{
    int Capacity { get; }

    InventoryItemInstance GetItem(int index);
    bool CanAddItem(ItemData_SO item, int amount);

    event Action<int, InventoryItemInstance> OnItemChanged;
}