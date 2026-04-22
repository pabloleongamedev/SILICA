using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData_SO itemData;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<InventoryController>();

        if (player == null) return;

        bool added = player.TryAddItem(itemData);

        if (added)
        {
            Destroy(gameObject);
        }
    }
}