public class InventoryItem
{
    public ItemData_SO data;
    public int x;
    public int y;

    public InventoryItem(ItemData_SO data, int x, int y)
    {
        this.data = data;
        this.x = x;
        this.y = y;
    }
}