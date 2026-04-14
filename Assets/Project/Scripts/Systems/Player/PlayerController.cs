using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;
    private InputSystem_Actions inputActions;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryDescription;
    private bool isInventoryOpen;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        mouseLook = GetComponentInChildren<MouseLook>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Jump.started += ctx => movementController.OnJumpStarted();
        inputActions.Player.Inventory.performed += ctx => ToggleInventory();

        inputActions.Player.Jetpack.performed += ctx => movementController.SetJetpack(true);
        inputActions.Player.Jetpack.canceled += ctx => movementController.SetJetpack(false);

        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
    }
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        inventoryPanel.SetActive(isInventoryOpen);
        //inventoryDescription.SetActive(true);

        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;
    }

    private void OnMove(InputAction.CallbackContext context) => movementController.SetMoveInput(context.ReadValue<Vector2>());

    private void OnSprint(InputAction.CallbackContext context) => movementController.SetSprint(context.ReadValueAsButton());

    private void OnLook(InputAction.CallbackContext context) => mouseLook.SetLookInput(context.ReadValue<Vector2>());

    private void OnDisable() => inputActions.Disable();
}