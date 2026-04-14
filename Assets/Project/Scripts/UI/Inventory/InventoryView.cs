using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private InventorySlotView slotPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private InventoryDragHandler dragHandler;
    [SerializeField] private DescriptionPanelView descriptionPanel;
    


    private InventorySystem inventory;
    private InventorySlotView[,] slotViews;
    private Vector2Int? dragStartSlot;

    private void Start()
    {
        inventory = inventoryController.GetInventory();

        BuildGrid();
        Refresh();

        inventory.OnInventoryChanged += Refresh;
        //ClearItemInfo();
    }

    private void BuildGrid()
    {
        var grid = inventory.GetGrid();

        slotViews = new InventorySlotView[grid.Width, grid.Height];

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var view = Instantiate(slotPrefab, gridParent);
                view.Init(this, x, y);

                slotViews[x, y] = view;
            }
        }
    }

    public void Refresh()
    {
        var grid = inventory.GetGrid();

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);
                slotViews[x, y].UpdateView(slot);
            }
        }
    }

    // 🔥 API para interacción
    public void OnSlotClicked(int x, int y)
    {
        Debug.Log($"Click slot {x},{y}");
    }

    public InventorySystem GetInventory()
    {
        return inventory;
    }
    public void OnItemDropped(int fromX, int fromY, int toX, int toY)
    {
        inventory.MoveItem(fromX, fromY, toX, toY);
    }
    public void StartDrag(int x, int y, Sprite icon)
    {
        dragStartSlot = new Vector2Int(x, y);
        dragHandler.StartDrag(icon);
    }
    public void UpdateDrag(Vector2 position)
    {
        dragHandler.UpdateDrag(position);
    }
    public void EndDrag()
    {
        dragStartSlot = null;
        dragHandler.EndDrag();
    }
    public void OnDrop(int toX, int toY)
    {
        if (dragStartSlot == null)
            return;

        var from = dragStartSlot.Value;

        inventory.MoveItem(from.x, from.y, toX, toY);

        dragStartSlot = null;
    }
    public bool IsDragging()
    {
        return dragStartSlot != null;
    }
    public void OnSlotSelected(int x, int y)
    {
        var slot = inventory.GetGrid().GetSlot(x, y);

        descriptionPanel.Show(slot);
    }
}