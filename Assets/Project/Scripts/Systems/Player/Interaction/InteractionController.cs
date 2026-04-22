using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        var interactable = detector.CurrentInteractable;

        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}