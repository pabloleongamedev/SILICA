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

        Debug.Log("INTERACT PRESSED");

        var interactable = detector.CurrentInteractable;

        if (interactable != null)
        {
            Debug.Log("INTERACTUANDO CON: " + interactable);
            interactable.Interact(context);
        }
        else
        {
            Debug.Log("NO HAY INTERACTUABLE");
        }
    }
}