public class InventorySlot
{
    public InventoryItemInstance Item;

    public bool IsEmpty => Item == null;

    public void SetItem(InventoryItemInstance item)
    {
        Item = item;
    }

    public void Clear()
    {
        Item = null;
    }
}