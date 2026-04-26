using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChemistryController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SeparationMethod_SO[] methods;
    [SerializeField] private SeparationDatabase_SO database;

    [Header("Refs")]
    [SerializeField] private MethodListView methodListView;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private ToolChemistryView toolView;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private InventoryListView inventoryListView;
    [SerializeField] private Button refineButton;

    private SeparationMethod_SO currentMethod;
    private CompoundDefinition_SO currentCompound;

    private IInventoryReadModel read;
    private IInventoryWriteModel write;

    private ChemistrySystem system;

    private void Awake()
    {
        system = new ChemistrySystem();
    }

    private void Start()
    {
        // =========================
        // INIT INVENTORY
        // =========================
        read = inventoryController.ReadModel;
        write = inventoryController.WriteModel;

        inventoryListView.Initialize(read);
        inventoryListView.OnItemDropped += HandleInventoryDrop;

        // =========================
        // INIT METHODS
        // =========================
        methodListView.Build(methods, OnMethodSelected);

        // =========================
        // TOOL EVENTS (CLAVE)
        // =========================
        toolView.OnItemCleared += HandleToolCleared;

        // =========================
        // BUTTON
        // =========================
        refineButton.onClick.AddListener(OnRefineClicked);

        UpdateButton();
    }
    private void OnEnable()
    {
        toolView.OnItemPlaced += HandleItemPlaced;
        toolView.OnItemCleared += HandleItemCleared;
    }

    private void OnDisable()
    {
        toolView.OnItemPlaced -= HandleItemPlaced;
        toolView.OnItemCleared -= HandleItemCleared;
    }

    private void OnDestroy()
    {
        inventoryListView.OnItemDropped -= HandleInventoryDrop;
        toolView.OnItemCleared -= HandleToolCleared;
    }

    // =========================================================
    // INVENTORY → TOOL
    // =========================================================
    private void HandleInventoryDrop(int fromIndex, int toIndex)
    {
        Debug.Log($"[ChemistryController] HandleInventoryDrop from {fromIndex}");

        var itemInstance = read.GetItem(fromIndex);

        if (itemInstance == null)
        {
            Debug.Log("[ChemistryController] itemInstance NULL");
            return;
        }

        var item = itemInstance.Data;

        Debug.Log($"[ChemistryController] Item: {item.name}");

        var compound = database.Get(item);

        if (compound == null)
        {
            Debug.Log("[ChemistryController] NO ES SEPARABLE");
            return;
        }

        if (toolView.GetItem() != null)
        {
            Debug.Log("[ChemistryController] TOOL OCUPADO");
            return;
        }

        Debug.Log("[ChemistryController] CONSUMIENDO ITEM");

        write.RemoveItem(item, 1);

        Debug.Log("[ChemistryController] SET TOOL");

        toolView.SetItem(item);

        currentCompound = compound;

        UpdateButton();
    }
private void HandleItemPlaced(ItemData_SO item)
{
    Debug.Log("[CONTROLLER] HandleItemPlaced LLAMADO");

    var compound = database.Get(item);

    if (compound == null)
    {
        Debug.Log("[CONTROLLER] Item no separable");

        toolView.Clear();
        return;
    }

    Debug.Log("[CONTROLLER] CONSUMIENDO INVENTARIO");

    write.RemoveItem(item, 1);

    currentCompound = compound;

    UpdateButton();
}
    private void HandleItemCleared()
    {
        if (currentCompound == null)
            return;

        Debug.Log("[ChemistryController] Devolviendo item");

        write.AddItem(currentCompound.inputItem, 1);

        currentCompound = null;

        UpdateButton();
    }
    // =========================================================
    // TOOL → INVENTORY (ROLLBACK)
    // =========================================================
    private void HandleToolCleared()
    {
        if (currentCompound == null)
            return;

        // 🔥 DEVOLVER ITEM
        write.AddItem(currentCompound.inputItem, 1);

        currentCompound = null;

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

        UpdateButton();
    }

    // =========================================================
    // VALIDACIÓN
    // =========================================================
    private void UpdateButton()
    {
        if (refineButton == null)
            return;

        refineButton.interactable = system.CanSeparate(
            currentCompound,
            currentMethod,
            read
        );
    }

    // =========================================================
    // EJECUCIÓN
    // =========================================================
    private void OnRefineClicked()
    {
        if (currentCompound == null || currentMethod == null)
            return;

        bool success = system.Execute(
            currentCompound,
            currentMethod,
            read,
            write
        );

        if (!success)
            return;

        // 🔥 LIMPIAR TOOL (NO DEVUELVE ITEM)
        toolView.Clear();

        currentCompound = null;

        UpdateButton();

        Debug.Log("SEPARACIÓN COMPLETADA");
    }
}