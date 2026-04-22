using UnityEngine;
using UnityEngine.UI;
<<<<<<< HEAD
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotView : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;


    private InventoryView inventoryView;
    private int x;
    private int y;

    private bool isDragging;

    public void Init(InventoryView view, int x, int y)
    {
        this.inventoryView = view;
        this.x = x;
        this.y = y;
    }

    public void UpdateView(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            icon.enabled = false;
            quantityText.text = "";
=======
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

    public Action<InventoryItemInstance> OnSlotClicked;
    public Action<int, int> OnItemDropped;

    public void Initialize(int index, InventoryDragHandler dragHandler)
    {
        this.index = index;
        this.dragHandler = dragHandler;
    }

    public void SetItem(InventoryItemInstance item, int amount)
    {
        currentItem = item;

        if (item == null)
        {
            icon.enabled = false;
            icon.sprite = null;
            stackText.text = "";
>>>>>>> 7ca46c4 (restore scripts interaction system)
            return;
        }

        icon.enabled = true;
<<<<<<< HEAD
        icon.sprite = slot.Item.Data.icon;

        int quantity = slot.Item.Quantity;
        Debug.Log("CANTIDAD: "+ quantity);
        quantityText.text = quantity.ToString();
    }

    // ================= CLICK =================

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return; // 🔥 FIX

        inventoryView.OnSlotSelected(x, y);
    }

    // ================= DRAG =================

    public void OnBeginDrag(PointerEventData eventData)
    {
        var slot = inventoryView.GetInventory().GetGrid().GetSlot(x, y);

        if (slot.IsEmpty)
            return;

        isDragging = true;

        // ❌ NO desactivar icon
       icon.color = new Color(1, 1, 1, 0.2f);

        inventoryView.StartDrag(x, y, slot.Item.Data.icon);
    }

public void OnDrag(PointerEventData eventData)
{
    inventoryView.UpdateDrag(eventData.position);
}

public void OnEndDrag(PointerEventData eventData)
{
    isDragging = false;

    // ❌ NO reactivar icon
    icon.color = new Color(1, 1, 1, 1f);

    inventoryView.EndDrag();
}

/*    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        icon.enabled = true;

        // 🔥 Detectar si no cayó en slot
        if (eventData.pointerEnter == null ||
            eventData.pointerEnter.GetComponent<InventorySlotView>() == null)
        {
            Debug.Log("Drop inválido");
        }

        inventoryView.EndDrag();
    }
*/
    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<InventorySlotView>();

        if (dragged == null)
        {
            return;
        }
        inventoryView.OnDrop(x, y);

=======
        icon.sprite = item.Data.icon;
        stackText.text = amount > 1 ? amount.ToString() : "";
        Debug.LogWarning($"{item.Data.displayName}");
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
>>>>>>> 7ca46c4 (restore scripts interaction system)
    }
}