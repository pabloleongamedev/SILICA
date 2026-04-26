using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;

    private int openPanels = 0;

    private void OnEnable()
    {
        GameplayEvents.OnInventoryToggle += HandlePanel;
        GameplayEvents.OnCraftingToggle += HandlePanel;
        GameplayEvents.OnChemistryToggle += HandlePanel;
    }

    private void OnDisable()
    {
        GameplayEvents.OnInventoryToggle -= HandlePanel;
        GameplayEvents.OnCraftingToggle -= HandlePanel;
        GameplayEvents.OnChemistryToggle -= HandlePanel;
    }

    private void HandlePanel(bool isOpen)
    {
        openPanels += isOpen ? 1 : -1;
        openPanels = Mathf.Max(0, openPanels);

        crosshair.SetActive(openPanels == 0);
    }
}