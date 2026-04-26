using UnityEngine;

public class ChemistryUIController : MonoBehaviour
{
    [SerializeField] private GameObject chemistryPanel;

    private void OnEnable()
    {
        GameplayEvents.OnChemistryToggle += Handle;
    }

    private void OnDisable()
    {
        GameplayEvents.OnChemistryToggle -= Handle;
    }

    private void Handle(bool isOpen)
    {
        Debug.Log("CHEMISTRY PANEL: " + isOpen);

        if (chemistryPanel != null)
            chemistryPanel.SetActive(isOpen);
    }
}