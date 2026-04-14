public class InventorySlot
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public InventoryItemInstance Item;

    public bool IsEmpty => Item == null;

    public InventorySlot(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetItem(InventoryItemInstance item)
    {
        Item = item;
    }

    public void Clear()
    {
        Item = null;
    }
}