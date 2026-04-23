using System;

public interface IInventoryReadModel
{
    int Capacity { get; }

    InventoryItemInstance GetItem(int index);

    event Action<int, InventoryItemInstance> OnItemChanged;
}