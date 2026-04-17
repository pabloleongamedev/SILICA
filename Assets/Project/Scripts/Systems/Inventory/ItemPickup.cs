using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData_SO itemData;
    [SerializeField] private int amount;

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