using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ¡Importante para el foco!

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playPanel; // Nuevo: Para las ranuras
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
        // Al iniciar, nos aseguramos de estar en el menú principal
        ShowMainMenu();
    }

    // --- MÉTODOS DE NAVEGACIÓN ---

    public void ShowMainMenu() => SwitchPanel(mainPanel, mainFirstButton);
    
    public void ShowPlayMenu() 
{
    SwitchPanel(playPanel, playFirstButton);
    
    // Buscamos todos los slots en la escena y los actualizamos
    SaveSlot[] slots = FindObjectsByType<SaveSlot>(FindObjectsSortMode.None);
    foreach (SaveSlot slot in slots)
    {
        slot.RefreshSlot();
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

    public void StartGame(int sceneIndex)
    {
        // Aquí podrías guardar cuál slot se eligió antes de cargar
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}