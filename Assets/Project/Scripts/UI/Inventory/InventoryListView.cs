using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryListView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InventoryListItemView itemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private InventoryDragHandler dragHandler;

    private IInventoryReadModel inventory;

    private List<InventoryListItemView> items = new List<InventoryListItemView>();

    public Action<int, int> OnItemDropped;
    public Action<InventoryItemInstance> OnItemClicked;

    public void Initialize(IInventoryReadModel inventory)
    {
        this.inventory = inventory;

        Build();

        inventory.OnItemChanged += UpdateSlot;
    }

    private void Build()
    {
        // limpiar por si reinicializas
        foreach (var item in items)
            Destroy(item.gameObject);

        items.Clear();

        for (int i = 0; i < inventory.Capacity; i++)
        {
            var itemView = Instantiate(itemPrefab, container);

            // 🔥 INYECCIÓN
            itemView.Initialize(i, dragHandler);

            itemView.OnItemDropped += HandleDrop;
            itemView.OnItemClicked += HandleClick;

            items.Add(itemView);

            var item = inventory.GetItem(i);
            itemView.SetItem(item);
        }
    }

    private void UpdateSlot(int index, InventoryItemInstance item)
    {
        if (index < 0 || index >= items.Count)
            return;

        items[index].SetItem(item);
    }

    private void HandleDrop(int fromIndex, int toIndex)
    {
        OnItemDropped?.Invoke(fromIndex, toIndex);
    }

    private void HandleClick(InventoryItemInstance item)
    {
        OnItemClicked?.Invoke(item);
    }
}