using TMPro;
using UnityEngine;

public class InteractionUIController : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text text;

    private bool forceHide; // 🔥 estado de override

    private void OnEnable()
    {
        if (detector != null)
            detector.OnInteractableChanged += HandleChanged;

        GameplayEvents.OnCraftingToggle += HandleCrafting;
        GameplayEvents.OnInventoryToggle += HandleInventory;
    }

    private void OnDisable()
    {
        if (detector != null)
            detector.OnInteractableChanged -= HandleChanged;

        GameplayEvents.OnCraftingToggle -= HandleCrafting;
        GameplayEvents.OnInventoryToggle -= HandleInventory;
    }

    // 🔥 FORZADO POR UI
    private void HandleCrafting(bool isOpen)
    {
        forceHide = isOpen;
        Refresh();
    }

    private void HandleInventory(bool isOpen)
    {
        forceHide = isOpen;
        Refresh();
    }

    // 🔹 Evento del detector
    private void HandleChanged(IInteractable interactable)
    {
        Refresh();
    }

    // 🔥 ÚNICO PUNTO DE DECISIÓN
    private void Refresh()
    {
        if (forceHide)
        {
            Hide();
            return;
        }

        var interactable = detector.CurrentInteractable;

        if (interactable == null)
        {
            Hide();
            return;
        }

        string msg = interactable.GetInteractionText();

        if (string.IsNullOrEmpty(msg))
        {
            Hide();
            return;
        }

        Show(msg);
    }

    private void Show(string msg)
    {
        panel.SetActive(true);
        text.text = msg;
    }

    private void Hide()
    {
        panel.SetActive(false);
    }
}