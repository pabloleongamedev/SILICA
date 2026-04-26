using TMPro;
using UnityEngine;

public class InteractionUIController : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text text;

    private bool forceHide; // 🔥 estado de override
    private int openUI = 0;

    private void OnEnable()
    {   
        if (detector != null)
            detector.OnInteractableChanged += HandleChanged;

        GameplayEvents.OnCraftingToggle += HandleUI;
        GameplayEvents.OnInventoryToggle += HandleUI;
        GameplayEvents.OnChemistryToggle += HandleUI; // 🔥 FALTA ESTO
    }

    private void OnDisable()
    {
        if (detector != null)
            detector.OnInteractableChanged -= HandleChanged;

        GameplayEvents.OnCraftingToggle -= HandleUI;
        GameplayEvents.OnInventoryToggle -= HandleUI;
        GameplayEvents.OnChemistryToggle -= HandleUI;
    }

    // 🔥 FORZADO POR UI
    private void HandleUI(bool isOpen)
    {
        openUI += isOpen ? 1 : -1;
        openUI = Mathf.Max(0, openUI);

        forceHide = openUI > 0;

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