using UnityEngine;
using UnityEngine.InputSystem;
public class InventoryTester : MonoBehaviour
{
    [SerializeField] private PlayerInventoryBridge playerInventory;

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            PrintInventory();
        }
    }

    private void PrintInventory()
    {
        var inventory = playerInventory.GetInventory();

        if (inventory == null)
        {
            Debug.LogError("Inventory NULL");
            return;
        }

        Debug.Log(inventory.GetDebugView());
    }
}