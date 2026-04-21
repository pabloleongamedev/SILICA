using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData_SO itemData;
    [SerializeField] private int amount;

    private InventorySystem inventory;

        public void Init(InventorySystem inv)
    {
        inventory = inv;
    }

    public void Interact()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory NULL en WorldItem");
            return;
        }

        inventory.AddItem(itemData, amount);
        Destroy(gameObject);
    }

    public string GetInteractionText()
    {
        return $"Presiona E para recoger {itemData.itemID}";
    }
}