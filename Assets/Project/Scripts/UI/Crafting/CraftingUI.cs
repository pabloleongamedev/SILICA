using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    private CraftingSystem craftingSystem;
    private RecipeData_SO[] currentRecipes;

    public void Open(CraftingStation station, InventorySystem inventory)
    {
        gameObject.SetActive(true);

        craftingSystem = new CraftingSystem(inventory);
        currentRecipes = station.GetRecipes();

        // TODO: poblar botones UI
    }

    public void CraftRecipe(RecipeData_SO recipe)
    {
        if (craftingSystem.Craft(recipe))
        {
            Debug.Log("Craft exitoso");
        }
        else
        {
            Debug.Log("Faltan materiales");
        }
    }
}