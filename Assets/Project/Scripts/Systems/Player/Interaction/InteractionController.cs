using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private InventorySystem inventorySystem;

    private InteractionContext context;

    private void Awake()
    {
        context = new InteractionContext(inventorySystem);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        var interactable = detector.CurrentInteractable;

        if (interactable != null)
        {
            interactable.Interact(context);
        }
    }
}