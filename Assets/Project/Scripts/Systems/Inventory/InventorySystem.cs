using UnityEngine;
public class InventorySystem
{
    private InventoryGrid grid;
    public event System.Action<InventoryItemInstance> OnItemAdded;
    public event System.Action OnInventoryFull;

    public InventorySystem(int width, int height)
    {
        grid = new InventoryGrid(width, height);
    }

    public bool AddItem(ItemData_SO itemData)
    {
        var instance = new InventoryItemInstance(itemData);

        bool result = grid.TryAddItem(instance);

        if (result)
        {
            OnItemAdded?.Invoke(instance);
        }
        else
        {
            OnInventoryFull?.Invoke();
        }
        return result;
    }

    public InventoryGrid GetGrid()
    {
        return grid;
    }

    public string GetDebugView()
    {
        var gridData = grid;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("=== INVENTORY ===");

        for (int y = gridData.Height - 1; y >= 0; y--) // invertido para verlo tipo UI
        {
            for (int x = 0; x < gridData.Width; x++)
            {
                var slot = gridData.GetSlot(x, y);

                if (slot.IsEmpty)
                {
                    sb.Append("[ EMPTY ]");
                }
                else
                {
                    sb.Append($"[ {slot.Item.Data.displayName} ]");
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

}