using UnityEngine;

[CreateAssetMenu(menuName = "Game/Crafting/Recipe")]
public class RecipeData_SO : ScriptableObject
{
    public string recipeID;

    [System.Serializable]
    public struct Ingredient
    {
        public ItemData_SO item;
        public int amount;
    }

    public Ingredient[] ingredients;

    public ItemData_SO result;
    public int resultAmount = 1;
}