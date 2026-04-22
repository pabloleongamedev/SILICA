using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData_SO itemData;
    [SerializeField] private int amount = 1;
    public System.Action<ItemPickup> OnPicked;

    public void Interact(InteractionContext context)
    {
        if (context == null || context.Inventory == null)
        {
            Debug.LogError("Inventory NULL en contexto");
            return;
        }

        context.Inventory.AddItem(itemData, amount);
        OnPicked?.Invoke(this);
        Destroy(gameObject);
    }

    public string GetInteractionText()
    {
        return $"Presiona E para recoger {itemData.itemID}";
        
    }
}