using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{
        private bool isCraftingTableOpen;
    

    public void Interact(InteractionContext context)
    {
        isCraftingTableOpen = !isCraftingTableOpen;

        GameplayEvents.OnCraftingToggle?.Invoke(isCraftingTableOpen);

        var playerState = FindFirstObjectByType<PlayerStateController>();

        if (playerState != null)
        {
            playerState.SetState(
                isCraftingTableOpen ? UIState.Crafting : UIState.None
            );
        }
    }

    public string GetInteractionText()
    {
        if (isCraftingTableOpen) return null;
        return "Presiona E para usar mesa de crafteo";
    }
}