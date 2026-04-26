using UnityEngine;
public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    private void OnEnable()
    {
        GameplayEvents.OnInventoryToggle += Handle;
    }

    private void OnDisable()
    {
        GameplayEvents.OnInventoryToggle -= Handle;
    }

    private void Handle(bool isOpen)
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);
    }
}