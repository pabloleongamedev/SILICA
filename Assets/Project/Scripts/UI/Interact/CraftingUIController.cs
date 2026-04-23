using UnityEngine;

public class CraftingUIController : MonoBehaviour
{
    [SerializeField] private GameObject craftingPanel;

    private void OnEnable()
    {
        GameplayEvents.OnCraftingToggle += Handle;
    }

    private void OnDisable()
    {
        GameplayEvents.OnCraftingToggle -= Handle;
    }

    private void Handle(bool isOpen)
    {
        Debug.Log("CRAFTING PANEL: " + isOpen);

        if (craftingPanel != null)
            craftingPanel.SetActive(isOpen);
    }
}