using UnityEngine;

public class ChemistryTable : MonoBehaviour, IInteractable
{
    private bool isOpen;

    public void Interact(InteractionContext context)
    {
        isOpen = !isOpen;

        GameplayEvents.OnChemistryToggle?.Invoke(isOpen);

        var playerState = FindFirstObjectByType<PlayerStateController>();

        if (playerState != null)
        {
            playerState.SetState(
                isOpen ? UIState.Chemistry : UIState.None
            );
        }
    }

    public string GetInteractionText()
    {
        if (isOpen) return null;
        return "Presiona E para usar refinador";
    }
}