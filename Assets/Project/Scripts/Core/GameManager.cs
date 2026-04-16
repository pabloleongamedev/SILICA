using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// GameManager Singleton: Gestiona el estado completo de la partida y auto-guardado.
/// 
/// CARACTERISTICAS:
/// - Acceso global: GameManager.Instance.SaveGame()
/// - Auto-guardado periódico durante el juego
/// - Carga/guardado transparente de datos complejos
/// - Persiste entre escenas (DontDestroyOnLoad)
/// 
/// FLUJO DE USO:
/// 1. Menú: GameManager.Instance.RefreshSaveStates() → muestra "Continuar" o "Nueva Partida"
/// 2. Cargar: GameManager.Instance.LoadGame(slotID)
/// 3. Juego: Auto-save cada X segundos automáticamente
/// 4. Guardar: GameManager.Instance.SaveGame() manualmente si es necesario
/// </summary>
public class GameManager : MonoBehaviour
{
    // ===== SINGLETON =====
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Patrón Singleton con DontDestroyOnLoad para persistencia entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Inicializar el controlador de guardado
        saveController = new SaveController();

        Debug.Log("[GameManager] Inicializado el GameManager Singleton");
    }

    // ===== VARIABLES =====

    [Header("Auto-Save Configuration")]
    [SerializeField] private float autoSaveInterval = 60f; // Guardar cada 60 segundos
    [SerializeField] private bool enableAutoSave = true;

    private SaveController saveController;
    private GameData currentGameData;
    private string currentSlotID = "1";
    
    private float timeSinceLastSave = 0f;
    private float sessionStartTime = 0f;
    private bool isInGame = false;

    // ===== CICLO DE VIDA =====

    private void Start()
    {
        sessionStartTime = Time.time;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // Auto-save periódico durante el juego
        if (isInGame && enableAutoSave && currentGameData != null)
        {
            timeSinceLastSave += Time.deltaTime;

            if (timeSinceLastSave >= autoSaveInterval)
            {
                AutoSave();
                timeSinceLastSave = 0f;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentGameData != null)
        {
            currentGameData.currentScene = scene.name;

            // Si no es el menú, estamos en juego
            isInGame = (scene.name != "Menu");

            if (isInGame)
            {
                Debug.Log($"[GameManager] Escena cargada: {scene.name}. Auto-save habilitado.");
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ===== MÉTODOS PÚBLICOS =====

    /// <summary>
    /// Carga una partida guardada desde disco
    /// </summary>
    public void LoadGame(string slotID)
    {
        GameData loadedData = saveController.LoadGame(slotID);

        if (loadedData != null)
        {
            currentGameData = loadedData;
            currentSlotID = slotID;
            sessionStartTime = Time.time; // Resetear sessionStartTime para nueva sesión
            timeSinceLastSave = 0f;

            Debug.Log($"[GameManager] Partida cargada del slot {slotID}");
            Debug.Log($"[GameManager] Posición guardada: {currentGameData.playerData.GetPosition()}");

            // Cargar la escena de la partida guardada
            SceneManager.LoadScene(currentGameData.currentScene);
        }
        else
        {
            Debug.LogError($"[GameManager] Error: No se pudo cargar la partida del slot {slotID}");
        }
    }

    /// <summary>
    /// Comienza una nueva partida en un slot específico
    /// </summary>
    public void CreateNewGame(string slotID)
    {
        currentGameData = GameData.CreateNewGame(slotID);
        currentSlotID = slotID;
        sessionStartTime = Time.time;
        timeSinceLastSave = 0f;
        isInGame = false; // Se establecerá a true en OnSceneLoaded

        Debug.Log($"[GameManager] Nueva partida creada en slot {slotID}");
        Debug.Log($"[GameManager] Posición inicial: {currentGameData.playerData.GetPosition()}");

        // Cargar la primera escena del juego
        //SceneManager.LoadScene(currentGameData.currentScene);
         SceneManager.LoadScene("Scene1");
    }

    /// <summary>
    /// Guarda manualmente la partida actual
    /// </summary>
    public void SaveGame()
    {
        if (currentGameData == null)
        {
            Debug.LogWarning("[GameManager] No hay partida activa para guardar");
            return;
        }

        // Actualizar datos del jugador antes de guardar
        UpdatePlayerData();

        // Guardar en disco
        saveController.SaveGame(currentGameData, currentSlotID);

        Debug.Log($"[GameManager] Partida guardada en slot {currentSlotID}");
    }

    /// <summary>
    /// Auto-guarda la partida sin mostrar logs (llamado periódicamente)
    /// </summary>
    private void AutoSave()
    {
        if (currentGameData == null)
            return;

        UpdatePlayerData();
        saveController.SaveGame(currentGameData, currentSlotID);

        Debug.Log($"[GameManager] AUTO-SAVE ejecutado en slot {currentSlotID}");
    }

    /// <summary>
    /// Verifica si existe un guardado en un slot específico
    /// </summary>
    public bool HasSaveFile(string slotID)
    {
        return saveController.HasSaveFile(slotID);
    }

    /// <summary>
    /// Obtiene información condensada de un guardado (para UI)
    /// </summary>
    public SaveInfo GetSaveInfo(string slotID)
    {
        return saveController.GetSaveInfo(slotID);
    }

    /// <summary>
    /// Obtiene información de todos los guardos (para menú)
    /// </summary>
    public SaveInfo[] GetAllSaveInfos()
    {
        return saveController.GetAllSaveInfos();
    }

    /// <summary>
    /// Recarga datos de guardos desde disco (útil al volver al menú)
    /// </summary>
    public void RefreshSaveStates()
    {
        Debug.Log("[GameManager] Estados de guardos refrescados");
    }

    /// <summary>
    /// Obtiene la partida actualmente cargada
    /// </summary>
    public GameData GetCurrentGameData()
    {
        return currentGameData;
    }

    /// <summary>
    /// Obtiene el ID del slot actual
    /// </summary>
    public string GetCurrentSlotID()
    {
        return currentSlotID;
    }

    // ===== MÉTODOS PARA SISTEMAS =====

    /// <summary>
    /// Registra la posición del jugador (llamado por PlayerController)
    /// </summary>
    public void UpdatePlayerPosition(Vector3 position)
    {
        if (currentGameData != null)
        {
            currentGameData.playerData.SetPosition(position);
        }
    }

    /// <summary>
    /// Registra la rotación del jugador (llamado por MouseLook)
    /// </summary>
    public void UpdatePlayerRotation(Quaternion rotation)
    {
        if (currentGameData != null)
        {
            currentGameData.playerData.SetRotation(rotation);
        }
    }

    /// <summary>
    /// Registra la salud del jugador
    /// </summary>
    public void UpdatePlayerHealth(int health, int maxHealth)
    {
        if (currentGameData != null)
        {
            currentGameData.playerData.health = health;
            currentGameData.playerData.maxHealth = maxHealth;
        }
    }

    /// <summary>
    /// Agrega un item al inventario guardado
    /// </summary>
    public void AddInventoryItem(string itemID, int gridX, int gridY, int quantity = 1)
    {
        if (currentGameData == null)
            return;

        currentGameData.inventoryItems.Add(new InventorySaveData
        {
            itemID = itemID,
            gridX = gridX,
            gridY = gridY,
            quantity = quantity
        });
    }

    /// <summary>
    /// Registra un elemento como escaneado
    /// </summary>
    public void RegisterScannedElement(string elementID)
    {
        if (currentGameData != null && !currentGameData.scannedElements.Contains(elementID))
        {
            currentGameData.scannedElements.Add(elementID);
        }
    }

    // ===== MÉTODOS PRIVADOS =====

    /// <summary>
    /// Actualiza todos los datos del jugador antes de guardar
    /// (Se llama automáticamente en SaveGame y AutoSave)
    /// </summary>
    private void UpdatePlayerData()
    {
        if (currentGameData == null)
            return;

        // Actualizar tiempo de juego
        float sessionTime = Time.time - sessionStartTime;
        currentGameData.UpdatePlayTime((int)sessionTime);

        // Si hay un controlador de jugador en la escena, sincronizar posición
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            Transform playerTransform = playerController.transform;
            currentGameData.playerData.SetPosition(playerTransform.position);
            currentGameData.playerData.SetRotation(playerTransform.rotation);
            
            Debug.Log($"[GameManager] UpdatePlayerData - Posición sincronizada: {playerTransform.position}");
        }
        else
        {
            Debug.LogWarning("[GameManager] PlayerController no encontrado en UpdatePlayerData");
        }
    }

    // ===== DEBUGGING =====

    /// <summary>
    /// Imprime el estado actual de la partida (para debugging)
    /// </summary>
    public string DebugGetGameState()
    {
        if (currentGameData == null)
            return "Sin partida activa";

        return $@"
        === GAME STATE ===
        Slot: {currentSlotID}
        Escena: {currentGameData.currentScene}
        Tiempo Jugado: {currentGameData.GetPlayTimeFormatted()}
        Guardado: {currentGameData.lastSaveTime}
        Items: {currentGameData.inventoryItems.Count}
        Elementos Escaneados: {currentGameData.scannedElements.Count}
        Posición Jugador: {currentGameData.playerData.GetPosition()}
        ==================";
            }
}
