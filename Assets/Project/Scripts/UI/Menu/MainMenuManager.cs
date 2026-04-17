using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ¡Importante para el foco!

/// <summary>
/// MainMenuManager: Gestiona la navegación del menú principal.
/// Sistema simplificado con una única partida guardada.
/// 
/// FLUJO:
/// 1. Usuario abre juego → ve Menu
/// 2. Click "Jugar" → muestra "Continuar" o "Nueva Partida"
/// 3. Click en el botón → carga TestMechanics automáticamente
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playPanel; // Panel con botón "Continuar" / "Nueva Partida"
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("First Selection Buttons")]
    [Tooltip("El botón que se resalta automáticamente al abrir cada panel")]
    [SerializeField] private Button mainFirstButton;
    [SerializeField] private Button playFirstButton;
    [SerializeField] private Button optionsFirstButton;
    [SerializeField] private Button creditsFirstButton;

    void Start()
    {
        // Asegurarse de que GameManager esté inicializado
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[MainMenuManager] Creando GameManager...");
            GameObject gmObject = new GameObject("GameManager");
            gmObject.AddComponent<GameManager>();
        }

        // Al iniciar, nos aseguramos de estar en el menú principal
        ShowMainMenu();
    }

    // --- MÉTODOS DE NAVEGACIÓN ---

    public void ShowMainMenu() => SwitchPanel(mainPanel, mainFirstButton);
    
    public void ShowPlayMenu() 
    {
        // Verificar si existe un guardado previo
        const string UNIQUE_SLOT = "1";
        
        if (GameManager.Instance.HasSaveFile(UNIQUE_SLOT))
        {
            // Si hay guardado: mostrar opciones "Continuar" / "Nueva Partida"
            SwitchPanel(playPanel, playFirstButton);
            
            GameManager.Instance.RefreshSaveStates();
            SaveSlot saveSlot = playPanel.GetComponentInChildren<SaveSlot>();
            if (saveSlot != null)
            {
                saveSlot.RefreshSlot();
                Debug.Log("[MainMenuManager] Guardado detectado - mostrando panel de opciones");
            }
        }
        else
        {
            // Si NO hay guardado: crear nueva partida y cargar directamente
            Debug.Log("[MainMenuManager] Primera vez - cargando TestMechanics automáticamente");
            GameManager.Instance.CreateNewGame(UNIQUE_SLOT);
        }
    }

    public void ShowOptions() => SwitchPanel(optionsPanel, optionsFirstButton);

    public void ShowCredits() => SwitchPanel(creditsPanel, creditsFirstButton);


    // --- LÓGICA DE ESCALABILIDAD Y ACCESIBILIDAD ---

    private void SwitchPanel(GameObject targetPanel, Button firstButton)
    {
        // 1. Desactivamos todos los paneles primero
        mainPanel.SetActive(false);
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        // 2. Activamos solo el panel objetivo
        targetPanel.SetActive(true);

        // 3. ACCESIBILIDAD: Forzamos el foco del teclado/mando
        if (firstButton != null)
        {
            // Limpiamos cualquier selección previa para evitar conflictos
            EventSystem.current.SetSelectedGameObject(null);
            // Asignamos el nuevo botón
            firstButton.Select();
        }
    }

    // --- ACCIONES FINALES ---

    /// <summary>
    /// Carga la escena de juego (TestMechanics)
    /// Se llama desde SaveSlot cuando el usuario selecciona una partida
    /// </summary>
    public void StartGame()
    {
        // El GameManager ya maneja la carga correcta de escena
        // Este método se puede usar como punto de extensión
        Debug.Log("[MainMenuManager] StartGame() llamado - GameManager manejará la carga");
    }

    /// <summary>
    /// Carga la escena de juego por nombre (alternativo)
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}