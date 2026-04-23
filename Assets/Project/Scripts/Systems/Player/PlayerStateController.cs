using UnityEngine;
public enum UIState
{
    None,
    Inventory,
    Crafting
}
public class PlayerStateController : MonoBehaviour
{
    private UIState currentState = UIState.None;
    private MovementController movementController;
    private MouseLook mouseLook;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        mouseLook = GetComponentInChildren<MouseLook>();
    }

    public void SetState(UIState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        bool isUI = newState != UIState.None;

        Cursor.lockState = isUI ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isUI;

        Time.timeScale = isUI ? 0f : 1f;

        if (movementController != null)
            movementController.SetInputEnabled(!isUI);

        if (mouseLook != null)
            mouseLook.enabled = !isUI;

        Debug.Log($"[PlayerState] State: {currentState}");
    }
    public bool CanInteract(IInteractable interactable)
    {
        switch (currentState)
        {
            case UIState.None:
                return true;

            case UIState.Inventory:
                return false;

            case UIState.Crafting:
                // 🔥 SOLO permitir interactuar con la mesa actual
                return interactable is CraftingTable;

            default:
                return false;
        }
    }

    public UIState GetState() => currentState;
}