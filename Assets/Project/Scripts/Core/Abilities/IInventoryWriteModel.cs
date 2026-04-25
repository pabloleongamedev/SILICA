public interface IInventoryWriteModel
{
    int AddItem(ItemData_SO data, int amount);
    void MoveItem(int fromIndex, int toIndex);
    void Clear();
}