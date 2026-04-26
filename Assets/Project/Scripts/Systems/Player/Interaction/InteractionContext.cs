public class InteractionContext
{
    public IInventoryReadModel InventoryRead { get; }
    public IInventoryWriteModel InventoryWrite { get; }

    public InteractionContext(InventorySystem inventory)
    {
        InventoryRead = inventory.ReadModel;
        InventoryWrite = inventory;
    }
}