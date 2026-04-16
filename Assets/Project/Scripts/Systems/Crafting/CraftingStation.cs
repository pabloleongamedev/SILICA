using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    [SerializeField] private RecipeData_SO[] recipes;

    public RecipeData_SO[] GetRecipes() => recipes;
}