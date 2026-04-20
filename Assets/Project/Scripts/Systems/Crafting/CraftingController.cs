using UnityEngine;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private RecipeDatabase_SO database;

    [Header("Views")]
    [SerializeField] private CraftingRecipeListView listView;
    [SerializeField] private CraftingRecipeDetailView detailView;

    private CraftingSystem system;

    private void Awake()
    {
        system = new CraftingSystem(database.recipes);

        // construir lista
        listView.Build(system.GetRecipes(), OnRecipeSelected);

        // suscribirse
        system.OnRecipeSelected += detailView.ShowRecipe;
    }

    private void OnRecipeSelected(RecipeData_SO recipe)
    {
        system.SelectRecipe(recipe);
    }
}