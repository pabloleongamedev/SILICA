using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("References")]
    private PlayerStateController stateController;
    private MovementController movementController;
    private MouseLook mouseLook;
    private InventoryController inventoryController;
    private InteractionDetector interactionDetector;
    

    private InputSystem_Actions inputActions;
    private InteractionContext interactionContext;

    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
        stateController = GetComponent<PlayerStateController>();
        movementController = GetComponent<MovementController>();
        interactionDetector = GetComponent<InteractionDetector>();
        mouseLook = GetComponentInChildren<MouseLook>();

        inputActions = new InputSystem_Actions();
    }
    private void Start()
    {
        var inventory = inventoryController.GetInventorySystem();

        if (inventory == null)
        {
            Debug.LogError("InventorySystem NULL en Start");
            return;
        }

        interactionContext = new InteractionContext(inventory);
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;

        inputActions.Player.Jump.started += ctx => movementController.OnJumpStarted();

        inputActions.Player.Jetpack.performed += ctx => movementController.SetJetpack(true);
        inputActions.Player.Jetpack.canceled += ctx => movementController.SetJetpack(false);

        inputActions.Player.Inventory.performed += ctx => ToggleInventory();

        inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // ------------------------
    // INPUT HANDLERS
    // ------------------------

    private void ToggleInventory()
    {
        var current = stateController.GetState();

        if (current != UIState.None && current != UIState.Inventory)
        {
            Debug.Log("No puedes abrir inventario durante crafting");
            return;
        }

        if (current == UIState.Inventory)
        {
            stateController.SetState(UIState.None);
            GameplayEvents.OnInventoryToggle?.Invoke(false);
            return;
        }

        stateController.SetState(UIState.Inventory);
        GameplayEvents.OnInventoryToggle?.Invoke(true);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        var interactable = interactionDetector.CurrentInteractable;

        if (interactable == null) return;

        // 🔥 DELEGACIÓN LIMPIA
        if (!stateController.CanInteract(interactable))
        {
            Debug.Log("Interact bloqueado por estado");
            return;
        }

        interactable.Interact(interactionContext);
    }

    private void OnMove(InputAction.CallbackContext ctx) =>
        movementController.SetMoveInput(ctx.ReadValue<Vector2>());

    private void OnSprint(InputAction.CallbackContext ctx) =>
        movementController.SetSprint(ctx.ReadValueAsButton());

    private void OnLook(InputAction.CallbackContext ctx) =>
        mouseLook.SetLookInput(ctx.ReadValue<Vector2>());
}