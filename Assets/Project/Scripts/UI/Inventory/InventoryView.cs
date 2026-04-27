using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InventorySlotView slotPrefab;
    [SerializeField] private Transform InventoryGridContainer;
    [SerializeField] private DescriptionPanelView descriptionPanel;
    [SerializeField] private InventoryDragHandler dragHandler;

    private IInventoryReadModel inventory;
    private List<InventorySlotView> slotViews = new List<InventorySlotView>();

    public Action<int, int> OnItemDropped;

    public void Initialize(IInventoryReadModel inventory)
    {
        this.inventory = inventory;

        Build();
        inventory.OnItemChanged += UpdateSlot;
    }

    private void Build()
    {
        
        for (int i = 0; i < inventory.Capacity; i++)
        {
            var slot = Instantiate(slotPrefab, InventoryGridContainer);

            // 🔥 INYECCIÓN
            slot.Initialize(i, dragHandler);

            slot.OnSlotClicked += HandleSlotClicked;
            slot.OnItemDropped += HandleItemDropped;

            slotViews.Add(slot);

            var item = inventory.GetItem(i);
            slot.SetItem(item, item != null ? item.Quantity : 0);
        }
    }

    private void UpdateSlot(int index, InventoryItemInstance item)
    {
        if (index < 0 || index >= slotViews.Count)
            return;

        // 🔥 SIEMPRE leer del modelo real
        var realItem = inventory.GetItem(index);

        slotViews[index].SetItem(realItem, realItem != null ? realItem.Quantity : 0);
    }

    private void HandleSlotClicked(InventoryItemInstance item)
    {
        if (descriptionPanel == null)
        {
            Debug.LogError("DescriptionPanel not assigned");
            return;
        }

        descriptionPanel.Show(item);
    }

    private void HandleItemDropped(int fromIndex, int toIndex)
    {
        OnItemDropped?.Invoke(fromIndex, toIndex);
    }

    public void ForceRefresh()
    {
        for (int i = 0; i < slotViews.Count; i++)
        {
            var item = inventory.GetItem(i);
            slotViews[i].SetItem(item, item != null ? item.Quantity : 0);
        }
    }
}