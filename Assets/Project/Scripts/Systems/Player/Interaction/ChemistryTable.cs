using UnityEngine;

public class ChemistryTable : MonoBehaviour, IInteractable
{
    public void Interact(InteractionContext context)
    {
        if (context.Inventory == null)
        {
            Debug.LogError("Inventory NULL");
            return;
        }

        Debug.Log("Abrir sistema químico con inventario");

        // aquí puedes abrir UI o validar ingredientes
    }

    public string GetInteractionText()
    {
        return "Presiona E para usar mesa química";
    }
}