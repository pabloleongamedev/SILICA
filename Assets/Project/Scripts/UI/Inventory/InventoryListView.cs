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
<<<<<<< HEAD

=======
>>>>>>> 7ca46c4 (restore scripts interaction system)
    private List<InventoryListItemView> items = new List<InventoryListItemView>();

    public Action<int, int> OnItemDropped;
    public Action<InventoryItemInstance> OnItemClicked;

    public void Initialize(IInventoryReadModel inventory)
    {
        this.inventory = inventory;

        Build();

<<<<<<< HEAD
=======
        // 🔥 FIX CRÍTICO: evitar múltiples suscripciones
        inventory.OnItemChanged -= UpdateSlot;
>>>>>>> 7ca46c4 (restore scripts interaction system)
        inventory.OnItemChanged += UpdateSlot;
    }

    private void Build()
    {
<<<<<<< HEAD
        // limpiar por si reinicializas
=======
        // limpiar
>>>>>>> 7ca46c4 (restore scripts interaction system)
        foreach (var item in items)
            Destroy(item.gameObject);

        items.Clear();

<<<<<<< HEAD
=======
        // 🔥 IMPORTANTE: SIEMPRE usar Capacity (1:1 con grid)
>>>>>>> 7ca46c4 (restore scripts interaction system)
        for (int i = 0; i < inventory.Capacity; i++)
        {
            var itemView = Instantiate(itemPrefab, container);

<<<<<<< HEAD
            // 🔥 INYECCIÓN
=======
>>>>>>> 7ca46c4 (restore scripts interaction system)
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

<<<<<<< HEAD
        items[index].SetItem(item);
=======
        // 🔥 FIX CRÍTICO: SIEMPRE consultar el modelo real
        var realItem = inventory.GetItem(index);

        items[index].SetItem(realItem);
        Debug.Log($"EVENT ITEM: {item}");
        Debug.Log($"REAL ITEM: {inventory.GetItem(index)}");
>>>>>>> 7ca46c4 (restore scripts interaction system)
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