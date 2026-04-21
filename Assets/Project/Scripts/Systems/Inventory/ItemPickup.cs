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
        return $"Presiona E para recoger {itemData.name}";
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<InventoryController>();

        if (controller == null)
            return;

        int remaining = controller.TryAddItem(itemData, amount);

        if (remaining <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            amount = remaining;
            Debug.Log("Inventario lleno parcialmente, quedan: " + remaining);
        }
    }
}