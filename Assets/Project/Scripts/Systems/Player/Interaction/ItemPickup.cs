using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData_SO item;
    [SerializeField] private int amount = 1;

    public void Interact(InteractionContext context)
    {
        // 🔥 VALIDACIÓN (READ MODEL)
        if (!context.InventoryRead.CanAddItemsBatch((item, amount)))
        {
            Debug.Log("Inventario lleno");
            return;
        }

        // 🔥 INSERCIÓN (WRITE MODEL)
        context.InventoryWrite.AddItem(item, amount);

        // 🔥 DESTRUIR
        Destroy(gameObject);
    }

    public string GetInteractionText()
    {
        return item != null ? $"Presiona E para recoger {item.itemID}" : "Recoger objeto";
    }
}