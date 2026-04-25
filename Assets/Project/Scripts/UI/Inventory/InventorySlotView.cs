using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class InventorySlotView : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IDropHandler, IPointerClickHandler
{
    private int index;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI stackText;

    private InventoryDragHandler dragHandler;

    private InventoryItemInstance currentItem;
    private int currentAmount; // 🔥 FIX

    public Action<InventoryItemInstance> OnSlotClicked;
    public Action<int, int> OnItemDropped;

    public void Initialize(int index, InventoryDragHandler dragHandler)
    {
        this.index = index;
        this.dragHandler = dragHandler;

        
    }

    public void SetItem(InventoryItemInstance item, int amount)
    {
    

        if (item == null)
        {
            icon.enabled = false;
            icon.sprite = null;
            stackText.text = "";
            return;
        }
            // 🔥 EVITA REDRAW INNECESARIO
        if (currentItem == item && currentAmount == amount)
            return;

        currentItem = item;
        currentAmount = amount;

        icon.enabled = true;
        icon.sprite = item.Data.icon;
        stackText.text = amount > 1 ? amount.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;
        OnSlotClicked?.Invoke(currentItem);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null || dragHandler == null) return;
        dragHandler.StartDrag(currentItem.Data.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragHandler == null) return;
        dragHandler.UpdateDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragHandler == null) return;
        dragHandler.EndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var fromSlot = eventData.pointerDrag?.GetComponent<InventorySlotView>();
        if (fromSlot == null) return;

        OnItemDropped?.Invoke(fromSlot.index, this.index);
    }
}