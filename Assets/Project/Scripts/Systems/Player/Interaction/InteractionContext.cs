public class InteractionContext
{
    public InventorySystem Inventory;

    public IInventoryReadModel InventoryRead;
    public IInventoryWriteModel InventoryWrite;

    public InteractionContext(InventorySystem inventory)
    {
        Inventory = inventory;
    }
}