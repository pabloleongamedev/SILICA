using UnityEngine;

public class ChemistryTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingUI;

    public void Interact(InteractionContext context)
    {
        if (context.InventoryRead == null)
        {
            Debug.LogError("Inventory NULL");
            return;
        }

        if (craftingUI != null)
            craftingUI.SetActive(true);
    }

    public string GetInteractionText()
    {
        return "Presiona E para usar mesa química";
    }
}