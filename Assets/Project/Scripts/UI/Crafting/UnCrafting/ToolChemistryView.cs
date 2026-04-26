using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ToolChemistryView : MonoBehaviour, IDropHandler
{
    [SerializeField] private ChemistrySlotView slot;

    private ItemData_SO currentItem;

    public Action<ItemData_SO> OnItemPlaced;
    public Action OnItemCleared;

    public void OnDrop(PointerEventData eventData)
    {
        var dragGO = eventData.pointerDrag;
        if (dragGO == null) return;

        var itemView = dragGO.GetComponent<InventoryListItemView>();
        if (itemView == null) return;

        var itemInstance = itemView.GetItem();
        if (itemInstance == null) return;

        if (currentItem != null)
        {
            Debug.Log("[Tool] Slot ocupado");
            return;
        }

        SetItem(itemInstance.Data);
    }

    public void SetItem(ItemData_SO item)
    {
        currentItem = item;
        slot.SetItem(item);

        OnItemPlaced?.Invoke(item);
    }

    public void Clear()
    {
        if (currentItem == null)
            return;

        currentItem = null;
        slot.Clear();

        OnItemCleared?.Invoke();
    }

    public ItemData_SO GetItem() => currentItem;
}