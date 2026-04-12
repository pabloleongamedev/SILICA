using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [Header("Configuración")]
    public int slotID;
    [SerializeField] private TextMeshProUGUI _textLabel;

    private bool _hasData = false;

    void Start()
    {
        UpdateSlotVisual();
    }

    public void UpdateSlotVisual()
    {
        // Para revisar si el archivo "save1.json" existe
        if (_hasData)
        {
            _textLabel.text = $"Slot {slotID} - Continuar";
        }
        else
        {
            _textLabel.text = $"Slot {slotID} - Nueva Partida";
        }
    }

    public void OnSlotPressed()
    {
        if (_hasData)
        {
            Debug.Log($"Cargando partida del Slot {slotID}...");
            // Aquí llamarías a cargar los datos
        }
        else
        {
            Debug.Log($"Creando nueva partida en el Slot {slotID}...");
            // Iniciar la introducción del juego
        }
    }
}