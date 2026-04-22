using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingUI;
    
    private bool isCraftingTableOpen;
    private PlayerController player;


        private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>(); // simple y funcional
    }

    public void Interact(InteractionContext context)
    {
        if (craftingUI == null)
        {
            Debug.LogError("Crafting UI no asignada");
            return;
        }
        isCraftingTableOpen = !isCraftingTableOpen;
        player.TogglePause(craftingUI, isCraftingTableOpen);
    }

    public string GetInteractionText()
    {
        // 🔥 CLAVE
        if (isCraftingTableOpen) return null;

        return "Presiona E para usar mesa de crafteo";
    }
}