using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
<<<<<<< HEAD
=======
    [SerializeField] private InventorySystem inventorySystem;

    private InteractionContext context;

    private void Awake()
    {
        context = new InteractionContext(inventorySystem);
    }
>>>>>>> 7ca46c4 (restore scripts interaction system)

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

<<<<<<< HEAD
<<<<<<< HEAD
=======
        Debug.Log("INTERACT PRESSED");

>>>>>>> 7ca46c4 (restore scripts interaction system)
=======
>>>>>>> 52f1bdd (Sistemas de Inventario, Crafteo e Interaccion completos)
        var interactable = detector.CurrentInteractable;

        if (interactable != null)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            interactable.Interact();
=======
            Debug.Log("INTERACTUANDO CON: " + interactable);
            interactable.Interact(context);
        }
        else
        {
            Debug.Log("NO HAY INTERACTUABLE");
>>>>>>> 7ca46c4 (restore scripts interaction system)
        }
=======
            interactable.Interact(context);
        }
>>>>>>> 52f1bdd (Sistemas de Inventario, Crafteo e Interaccion completos)
    }
}