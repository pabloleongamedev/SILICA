using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerController: Controla la entrada del jugador.
/// INTEGRACIÓN CON GAMEMANAGER: Sincroniza posición/rotación para guardado automático.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private MovementController movementController;
    private InputSystem_Actions inputActions;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryDescription;
    private bool isInventoryOpen;

    private float lastGameManagerUpdateTime = 0f;
    private float gameManagerUpdateInterval = 0.5f; // Actualizar cada 0.5 segundos

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        mouseLook = GetComponentInChildren<MouseLook>();
        inputActions = new InputSystem_Actions();
        
        // No resetear la posición del jugador - dejar que GameRestorer la restaure
        // si es una partida cargada
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

        // UI
        inventoryPanel.SetActive(isInventoryOpen);

        // Cursor
        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;

        // Pausa del juego
        Time.timeScale = isInventoryOpen ? 0f : 1f;

        // Bloquear cámara
        if (mouseLook != null)
            mouseLook.enabled = !isInventoryOpen;

        // Bloquear movimiento
        if (movementController != null)
            movementController.SetInputEnabled(!isInventoryOpen);
    }

    private void Update()
    {
        // Sincronizar con GameManager periódicamente para auto-save
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

    private void OnMove(InputAction.CallbackContext context) => movementController.SetMoveInput(context.ReadValue<Vector2>());

    private void OnSprint(InputAction.CallbackContext context) => movementController.SetSprint(context.ReadValueAsButton());

    private void OnLook(InputAction.CallbackContext context) => mouseLook.SetLookInput(context.ReadValue<Vector2>());

    private void OnDisable() => inputActions.Disable();

    /// <summary>
    /// Se llama cuando el jugador presiona una tecla de guardado manual (Ctrl+S)
    /// </summary>
    public void RequestManualSave()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            Debug.Log("[PlayerController] Guardado manual ejecutado");
        }
    }
}