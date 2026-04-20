using System;
using System.Collections.Generic;

public class CraftingSystem
{
    private List<RecipeData_SO> recipes;

    public Action<RecipeData_SO> OnRecipeSelected;

    private RecipeData_SO currentRecipe;

    public CraftingSystem(List<RecipeData_SO> recipes)
    {
        this.recipes = recipes;
    }

    public List<RecipeData_SO> GetRecipes()
    {
        return recipes;
    }

    public void SelectRecipe(RecipeData_SO recipe)
    {
        currentRecipe = recipe;
        OnRecipeSelected?.Invoke(recipe);
    }

    public RecipeData_SO GetCurrentRecipe()
    {
        return currentRecipe;
    }
}