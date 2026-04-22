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

<<<<<<< HEAD
        inventorySystem = inventoryController.GetInventory();
=======
        inventorySystem = inventoryController.GetInventorySystem();
>>>>>>> 7ca46c4 (restore scripts interaction system)

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
        if (!system.TryPlaceItem(slotIndex, item, inventorySystem))
            return;

        toolView.SetItemInSlot(slotIndex, item);

        if (system.IsRecipeComplete())
            Debug.Log("RECETA COMPLETA");
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
        var recipe = system.GetCurrentRecipe(); // usa tu método real

        if (recipe == null)
        {
            Debug.Log("No hay receta seleccionada");
            return;
        }

        if (!toolView.IsComplete(recipe))
        {
            Debug.Log("Receta incompleta");
            return;
        }

        inventorySystem.AddItem(recipe.result, recipe.resultAmount);

        toolView.ConsumeAllSlots();

        Debug.Log("CRAFT COMPLETADO");
    }
}