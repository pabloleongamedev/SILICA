using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CraftingController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RecipeDatabase_SO database;

    [Header("Refs")]
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private ToolCraftingView toolView;
    [SerializeField] private CraftingRecipeListView listView;
    [SerializeField] private CraftingRecipeDetailView detailView;
    [SerializeField] private Button craftButton;

    private CraftingSystem system;

    private IInventoryReadModel read;
    private IInventoryWriteModel write;

    private void Awake()
    {
        system = new CraftingSystem();
    }

    private void Start()
    {
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController no asignado");
            return;
        }

        read = inventoryController.ReadModel;
        write = inventoryController.WriteModel;

        BuildRecipesUI();
        UpdateCraftButton();
    }

    private void OnEnable()
    {
        toolView.OnItemDroppedInSlot += HandleItemDropped;
        toolView.OnItemDragOut += HandleItemReturned;
        craftButton.onClick.AddListener(OnCraftClicked);
    }

    private void OnDisable()
    {
        toolView.OnItemDroppedInSlot -= HandleItemDropped;
        toolView.OnItemDragOut -= HandleItemReturned;
        craftButton.onClick.RemoveListener(OnCraftClicked);

        // ROLLBACK GLOBAL
        if (write != null)
        {
            system.ClearAll(write);
            toolView.Clear();
        }
    }

    // =========================
    // UI BUILD
    // =========================
    private void BuildRecipesUI()
    {
        if (listView == null || database == null)
            return;

        listView.Build(database.recipes, OnRecipeSelected);
    }

    private void OnRecipeSelected(RecipeData_SO recipe)
    {
        // 🔥 DEVOLVER ITEMS ANTES DE CAMBIAR
        system.ClearAll(write);

        system.SetRecipe(recipe);

        toolView.Clear();

        if (detailView != null)
            detailView.ShowRecipe(recipe);

        UpdateCraftButton();
    }

    // =========================
    // DRAG & DROP
    // =========================
    private void HandleItemDropped(int slotIndex, ItemData_SO item)
    {
        if (!system.TryPlaceItem(slotIndex, item,read, write))
            return;

        toolView.SetItemInSlot(slotIndex, item);

        UpdateCraftButton();
    }
    private void HandleItemReturned(int slotIndex, ItemData_SO item)
    {
        system.ClearSlot(slotIndex, write);
        toolView.ClearSlot(slotIndex);

        UpdateCraftButton();
    }
    // =========================
    // VALIDACIÓN REAL (CLAVE)
    // =========================

    private void UpdateCraftButton()
    {
        if (craftButton == null)
            return;

        craftButton.interactable = system.IsRecipeComplete();
    }

    // =========================
    // EJECUCIÓN REAL
    // =========================
    private void OnCraftClicked()
    {
        var recipe = system.GetCurrentRecipe();

        if (recipe == null)
            return;

        if (!system.IsRecipeComplete())
        {
            Debug.Log("Receta incompleta");

            // rollback
            system.ClearAll(write);
            toolView.Clear();

            UpdateCraftButton();
            return;
        }

        // PRODUCIR RESULTADO
        write.AddItem(recipe.result, recipe.resultAmount);

        // LIMPIAR SIN DEVOLVER (IMPORTANTE)
        system.ClearAllNoReturn();

        toolView.Clear();

        UpdateCraftButton();

        Debug.Log("CRAFT COMPLETADO");
    }
}