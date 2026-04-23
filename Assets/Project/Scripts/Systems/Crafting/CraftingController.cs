using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private RecipeDatabase_SO database;
    [SerializeField] private ToolCraftingView toolView;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CraftingRecipeListView listView;
    [SerializeField] private CraftingRecipeDetailView detailView;
    [SerializeField] private Button craftButton;

    private CraftingSystem system;
    private InventorySystem inventorySystem;

    private void Awake()
    {
        system = new CraftingSystem(database.recipes);
    }
    private void Start()
    {
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController no asignado");
            return;
        }

        inventorySystem = inventoryController.GetInventorySystem();

        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem sigue siendo NULL en Start");
        }
    }

    private void OnEnable()
    {
        toolView.OnItemDroppedInSlot += HandleItemDropped;
        toolView.OnItemDragOut += HandleItemReturned;
        craftButton.onClick.AddListener(OnCraftClicked);

        BuildRecipesUI();
    }

    private void OnDisable()
    {
        toolView.OnItemDroppedInSlot -= HandleItemDropped;
        toolView.OnItemDragOut -= HandleItemReturned;
    }

    private void HandleItemDropped(int slotIndex, ItemData_SO item)
    {
        if (system.GetCurrentRecipe() == null)
        {
            Debug.Log("Selecciona una receta primero");
            return;
        }

        if (!system.TryPlaceItem(slotIndex, item, inventorySystem))
            return;

        toolView.SetItemInSlot(slotIndex, item);

        UpdateCraftButton();
    }
    private void UpdateCraftButton()
    {
        if (craftButton == null) return;

        craftButton.interactable = system.IsRecipeComplete();
    }
    private void HandleItemReturned(int slotIndex, ItemData_SO item)
    {
        Debug.Log("RETURN ITEM");

        int amount = system.GetRequiredAmount(item);

        if (amount <= 0) return;

        inventorySystem.AddItem(item, amount);

        system.ClearSlot(slotIndex);
        toolView.ClearSlot(slotIndex);
    }
    private void BuildRecipesUI()
    {
        if (listView == null)
        {
            Debug.LogError("ListView not assigned");
            return;
        }

        if (database == null)
        {
            Debug.LogError("Database not assigned");
            return;
        }

        Debug.Log("BUILD RECIPES UI");

        listView.Build(database.recipes, OnRecipeSelected);
    }
    private void OnRecipeSelected(RecipeData_SO recipe)
    {
        system.ReturnAllItems(inventorySystem);
        toolView.Clear();
        Debug.Log("RECIPE SELECTED: " + recipe.name);

        system.SetRecipe(recipe);

        if (detailView != null)
        {
            detailView.ShowRecipe(recipe); 
        }

        toolView.Clear();
    }
    private void OnCraftClicked()
    {
        var recipe = system.GetCurrentRecipe();

        if (recipe == null)
        {
            Debug.Log("No hay receta seleccionada");
            return;
        }

        if (!system.IsRecipeComplete())
        {
            Debug.Log("Receta incompleta");

            // DEVOLVER ITEMS
            system.ReturnAllItems(inventorySystem);
            toolView.Clear();

            return;
        }

        inventorySystem.AddItem(recipe.result, recipe.resultAmount);

        toolView.ConsumeAllSlots();
        system.ClearAll();

        Debug.Log("CRAFT COMPLETADO");
    }
    

}