using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChemistryController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SeparationMethod_SO[] methods;
    [SerializeField] private SeparationDatabase_SO database;

    [Header("UI")]
    [SerializeField] private MethodListView methodListView;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private ToolChemistryView toolView;
    [SerializeField] private InventoryListView inventoryListView;
    [SerializeField] private Button refineButton;

    [Header("Refs")]
    [SerializeField] private InventoryController inventoryController;

    private SeparationMethod_SO currentMethod;
    private CompoundDefinition_SO currentCompound;

    private IInventoryReadModel read;
    private IInventoryWriteModel write;

    private ChemistrySystem system;

    // 🔥 evita doble rollback
    private bool isCleaning;

    private void Awake()
    {
        system = new ChemistrySystem();
    }

    private void Start()
    {
        read = inventoryController.ReadModel;
        write = inventoryController.WriteModel;

        inventoryListView.Initialize(read);
        inventoryListView.OnItemDropped += HandleInventoryDrop;

        methodListView.Build(methods, OnMethodSelected);

        toolView.OnItemPlaced += HandleItemPlaced;
        toolView.OnItemCleared += HandleItemCleared;

        refineButton.onClick.AddListener(OnRefineClicked);

        UpdateButton();
    }

    private void OnEnable()
    {
        GameplayEvents.OnUIStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameplayEvents.OnUIStateChanged -= HandleStateChanged;
    }

    private void OnDestroy()
    {
        inventoryListView.OnItemDropped -= HandleInventoryDrop;
        toolView.OnItemPlaced -= HandleItemPlaced;
        toolView.OnItemCleared -= HandleItemCleared;
    }

    // =========================================================
    // STATE CONTROL (REEMPLAZA OnChemistryToggle)
    // =========================================================
    private void HandleStateChanged(UIState state)
    {
        // 🔥 si salimos del estado Chemistry → cleanup
        if (state != UIState.Chemistry)
        {
            Cleanup();
        }
    }

    private void Cleanup()
    {
        Debug.Log("[ChemistryController] Cleanup");

        if (currentCompound == null && toolView.GetItem() == null)
            return;

        isCleaning = true;

        // 🔥 rollback seguro
        if (currentCompound != null)
        {
            write.AddItem(currentCompound.inputItem, 1);
        }

        toolView.Clear();

        currentCompound = null;
        currentMethod = null;

        isCleaning = false;

        Notify("Refinador limpiado", NotificationType.Info);

        UpdateButton();
    }

    // =========================================================
    // INVENTORY → TOOL
    // =========================================================
    private void HandleInventoryDrop(int fromIndex, int toIndex)
    {
        var itemInstance = read.GetItem(fromIndex);

        if (itemInstance == null)
        {
            Notify("Slot vacío", NotificationType.Warning);
            return;
        }

        var item = itemInstance.Data;
        var compound = database.Get(item);

        if (compound == null)
        {
            Notify("Este elemento no se puede separar", NotificationType.Warning);
            return;
        }

        if (toolView.GetItem() != null)
        {
            Notify("El refinador ya está ocupado", NotificationType.Warning);
            return;
        }

        toolView.SetItem(item);
    }

    // =========================================================
    // TOOL EVENTS
    // =========================================================
    private void HandleItemPlaced(ItemData_SO item)
    {
        var compound = database.Get(item);

        if (compound == null)
        {
            Notify("Elemento no se puede refinar", NotificationType.Error);
            toolView.Clear();
            return;
        }

        write.RemoveItem(item, 1);

        currentCompound = compound;

        Notify(item.itemID + " listo para refinar", NotificationType.Info);

        UpdateButton();
    }

    private void HandleItemCleared()
    {
        if (isCleaning) return;

        if (currentCompound == null) return;

        write.AddItem(currentCompound.inputItem, 1);

        currentCompound = null;

        Notify("Elemento devuelto al inventario", NotificationType.Info);

        UpdateButton();
    }

    // =========================================================
    // METHOD SELECT
    // =========================================================
    private void OnMethodSelected(SeparationMethod_SO method)
    {
        currentMethod = method;

        if (descriptionText != null)
            descriptionText.text = method.description;

        Notify("Método seleccionado: " + method.methodName, NotificationType.Info);

        UpdateButton();
    }

    // =========================================================
    private void UpdateButton()
    {
        if (refineButton == null)
            return;

        // 🔥 siempre activo (validación ocurre al hacer click)
        refineButton.interactable = true;
    }

    // =========================================================
    // EJECUCIÓN
    // =========================================================
    private void OnRefineClicked()
    {
        if (currentCompound == null)
        {
            Notify("Debes colocar un compuesto en el refinador", NotificationType.Warning);
            return;
        }

        if (currentMethod == null)
        {
            Notify("Debes seleccionar un método de separación", NotificationType.Warning);
            return;
        }

        if (currentCompound.requiredMethod != currentMethod)
        {
            Notify("Método incorrecto para este compuesto", NotificationType.Error);
            return;
        }

        if (!system.CanSeparate(currentCompound, currentMethod, read))
        {
            Notify("No hay espacio en el inventario", NotificationType.Warning);
            return;
        }

        bool success = system.Execute(
            currentCompound,
            currentMethod,
            read,
            write
        );

        if (!success)
        {
            Notify("No se pudo refinar", NotificationType.Error);
            return;
        }
  
        //  NOTIFICACIÓN A QUEST SYSTEM (CORRECTO
        foreach (var output in currentCompound.outputs)
        {
            QuestEvents.OnItemRefined?.Invoke(output.item, output.amount);
        }

        //  LIMPIEZA
        toolView.Clear();
        currentCompound = null;

        Notify("Elemento refinado con éxito", NotificationType.Success);
    }

    // =========================================================
    private void Notify(string msg, NotificationType type)
    {
        Debug.Log("[Chemistry] " + msg);

        GameplayEvents.OnNotification?.Invoke(new NotificationData
        {
            message = msg,
            type = type
        });
    }
}