/*using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections; // ¡Importante para el foco!

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

    [Header("UI")]
    [SerializeField] private GameObject loadingImage;

    [Header("Settings")]
    [SerializeField] private float delay = 4f;
    [SerializeField] private int sceneIndex = 1;

    [Header("UI References")]
    Button playButton;
    HUDManager hUDManager;


    void Start()
    {
      /*  // Asegurarse de que GameManager esté inicializado
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[MainMenuManager] Creando GameManager...");
            GameObject gmObject = new GameObject("GameManager");
            gmObject.AddComponent<GameManager>();
        }

        // Al iniciar, nos aseguramos de estar en el menú principal
        ShowMainMenu();*/
        //StartCoroutine(LoadSceneRoutine());
   /* }

    public void LoadSceneFromButton()
    {
        StartCoroutine(LoadSceneRoutine());
    }   

       private IEnumerator LoadSceneRoutine()
    {
        // 🔥 Mostrar imagen
        if (loadingImage != null)
            loadingImage.SetActive(true);

        // 🔥 Esperar
        yield return new WaitForSeconds(delay);

        // 🔥 Cargar escena
        SceneManager.LoadScene(sceneIndex);
    }

    // --- MÉTODOS DE NAVEGACIÓN ---

    public void ShowMainMenu() => SwitchPanel(mainPanel, mainFirstButton);
    
    public void ShowPlayMenu() 
    {
        // Verificar si existe un guardado previo
//  const string UNIQUE_SLOT = "1";
        
/*if (GameManager.Instance.HasSaveFile(UNIQUE_SLOT))
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
        }*/
   /* }

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
       // Debug.Log("[MainMenuManager] StartGame() llamado - GameManager manejará la carga");
    }

    /// <summary>
    /// Carga la escena de juego por nombre (alternativo)
    /// </summary>
/*    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void LoadGameSceneAndStartTimer()
    {
        StartCoroutine(LoadSceneAndStartTimerCoroutine());
    }

    private IEnumerator LoadSceneAndStartTimerCoroutine()
    {
        // Cambia "1" por el índice o nombre de tu escena de juego si es distinto
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        // Esperar hasta que la escena termine de cargar
        while (!op.isDone)
            yield return null;

        // Esperar un frame para asegurarnos de que Awake/Start de los objetos de la escena ya corrieron
        yield return null;

        // Buscar el HUDManager en la escena cargada y arrancar el timer
        // HUDManager hud = FindFirstObjectByType<HUDManager>();
        // if (hud != null && hud.missionTimer != null)
        // {
        //     hud.StartMission();
        //     Debug.Log("[MainMenuManager] HUDManager encontrado y MissionTimer iniciado.");
        // }
        // else
        // {
        //     Debug.LogWarning("[MainMenuManager] No se encontró HUDManager o MissionTimer en la escena cargada.");
        // }
    }
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loadingImage;

    [Header("Settings")]
    [SerializeField] private float delay = 4f;
    [SerializeField] private int sceneIndex = 1;

    // ❌ Quitamos Start()

    // ✅ FUNCIÓN PARA BOTÓN
    public void LoadSceneFromButton()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        // 🔥 Mostrar imagen
        if (loadingImage != null)
            loadingImage.SetActive(true);

        // 🔥 Esperar
        yield return new WaitForSeconds(delay);

        // 🔥 Cargar escena
        SceneManager.LoadScene(sceneIndex);
    }
}