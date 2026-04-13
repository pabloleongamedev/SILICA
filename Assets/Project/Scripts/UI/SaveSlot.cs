using UnityEngine;
using TMPro;
using System.IO;

public class SaveSlot : MonoBehaviour
{
    [Header("Configuración")]
    public int slotID;
    [SerializeField] private TextMeshProUGUI _textLabel;

    private bool _hasData = false;
    string _fullPath; // Guardar la ruta completa

    void Awake()
    {
        // Construir la ruta del archivo al iniciar
        // Se usa el ID para que cada slot busque su propio archivo (save1.json, save2.json ...)
        _fullPath = Path.Combine(Application.persistentDataPath, $"save_{slotID}.json");
    }
    void Start()
    {
        RefreshSlot();
    }

    public void RefreshSlot()
    {
        // Preguntar al disco duro si el archivo existe
        _hasData = File.Exists(_fullPath);
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
            Debug.Log($"Cargando partida desde {_fullPath}...");
            // Lógica para cargar el JSON
        }
        else
        {
            Debug.Log($"No existen datos. Creando aventura desde cero ...");
            // Lógica para crear el archivo inicial
        }
    }
}