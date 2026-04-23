using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject crosshair;
    

    private void Start()
    {
        HandleUI(false);
    }

    private void OnEnable()
    {        
        GameplayEvents.OnInventoryToggle += HandleUI;
    }

    private void OnDisable()
    {
        GameplayEvents.OnInventoryToggle -= HandleUI;
    }

    private void HandleUI(bool isOpen)
    {
    
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        if (crosshair != null)
            crosshair.SetActive(!isOpen);
    }
}