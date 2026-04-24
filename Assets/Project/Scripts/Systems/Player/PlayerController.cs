using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerController: Controla la entrada del jugador. ////////// ESTO ES BASURA
/// </summary>
public class PlayerController : MonoBehaviour
{
    private MovementController movementController;
    private InputSystem_Actions inputActions;
    private InteractionDetector interactionDetector;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private GameObject inventoryPanel;
    //[SerializeField] private GameObject inventoryDescription;
    [SerializeField] private GameObject crosshair;

    // 🔥 YA LO NECESITAS para el contexto
    private InventoryController inventoryController;

    private InteractionContext interactionContext;

    private bool isInventoryOpen;
    bool isPaused = false; 

    private float lastGameManagerUpdateTime = 0f;
    private float gameManagerUpdateInterval = 0.5f;



    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        mouseLook = GetComponentInChildren<MouseLook>();
        inputActions = new InputSystem_Actions();
        interactionDetector = GetComponentInChildren<InteractionDetector>();
        inventoryController = GetComponent<InventoryController>();
    }
    private void Start()
    {
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController no asignado");
            return;
        }

        var inventory = inventoryController.GetInventorySystem();

        if (inventory == null)
        {
            Debug.LogError("InventorySystem sigue NULL en Start");
            return;
        }

        interactionContext = new InteractionContext(inventory);
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Jump.started += ctx => movementController.OnJumpStarted();
        inputActions.Player.Inventory.performed += ctx => CallInventory();

        inputActions.Player.Jetpack.performed += ctx => movementController.SetJetpack(true);
        inputActions.Player.Jetpack.canceled += ctx => movementController.SetJetpack(false);

        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        inputActions.Player.Interact.performed += OnInteract;

        Cursor.visible = false;
    }

    private void CallInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        TogglePause(inventoryPanel,isInventoryOpen );
        
    }

    public void TogglePause(GameObject panel, bool status)
    {
        isInventoryOpen = status;
        panel.SetActive(isInventoryOpen);
        crosshair.SetActive(!isInventoryOpen);
        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;

        Time.timeScale = isInventoryOpen ? 0f : 1f;

        if (mouseLook != null)
            mouseLook.enabled = !isInventoryOpen;

        if (movementController != null)
            movementController.SetInputEnabled(!isInventoryOpen);
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            lastGameManagerUpdateTime += Time.deltaTime;

            if (lastGameManagerUpdateTime >= gameManagerUpdateInterval)
            {
                GameManager.Instance.UpdatePlayerPosition(transform.position);
                GameManager.Instance.UpdatePlayerRotation(transform.rotation);
                lastGameManagerUpdateTime = 0f;
            }
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (interactionDetector == null)
        {
            Debug.LogError("InteractionDetector NULL");
            return;
        }

        if (interactionContext == null)
        {
            Debug.LogError("InteractionContext NULL");
            return;
        }

        var interactable = interactionDetector.CurrentInteractable;

        if (interactable != null)
        {
            Debug.Log("INTERACTUANDO CON: " + interactable);

            // 🔥 AQUÍ ESTÁ EL FIX FINAL
            interactable.Interact(interactionContext);
        }
        else
        {
            Debug.Log("NO HAY INTERACTUABLE");
        }
    }

    private void OnMove(InputAction.CallbackContext context) =>
        movementController.SetMoveInput(context.ReadValue<Vector2>());

    private void OnSprint(InputAction.CallbackContext context) =>
        movementController.SetSprint(context.ReadValueAsButton());

    private void OnLook(InputAction.CallbackContext context) =>
        mouseLook.SetLookInput(context.ReadValue<Vector2>());

    private void OnDisable() => inputActions.Disable();

    public void RequestManualSave()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            Debug.Log("[PlayerController] Guardado manual ejecutado");
        }
    }

    // Desactivar interacciones mientras se presente el menú de Pausa
    public void SetPaused(bool paused)
{
    isPaused = paused;
    if (paused)
    {
        movementController.SetMoveInput(Vector2.zero);
        mouseLook.enabled = false;
    }
    else
    {
        mouseLook.enabled = true;
    }
}
}