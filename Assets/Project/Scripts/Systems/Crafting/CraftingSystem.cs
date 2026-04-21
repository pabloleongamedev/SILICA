using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem
{
    private RecipeData_SO currentRecipe;

    private Dictionary<int, (ItemData_SO item, int amount)> slots
        = new Dictionary<int, (ItemData_SO, int)>();

    public CraftingSystem(List<RecipeData_SO> recipes)
    {
        if (recipes != null && recipes.Count > 0)
            currentRecipe = recipes[0];
    }

    public void SetRecipe(RecipeData_SO recipe)
    {
        currentRecipe = recipe;
        slots.Clear();
    }

    public bool TryPlaceItem(int slotIndex, ItemData_SO item, InventorySystem inventory)
    {
        if (currentRecipe == null) return false;

        var ingredient = currentRecipe.ingredients
            .Find(x => x.item.itemID == item.itemID);

        if (ingredient == null)
        {
            Debug.Log("Item no pertenece a la receta");
            return false;
        }

        int available = inventory.GetAmount(item);

        if (available < ingredient.amount)
        {
            Debug.Log("Cantidad insuficiente");
            return false;
        }

        inventory.RemoveItem(item, ingredient.amount);

        slots[slotIndex] = (item, ingredient.amount);

        Debug.Log($"Item colocado en slot {slotIndex}");

        return true;
    }
    public int GetRequiredAmount(ItemData_SO item)
    {
        var ingredient = currentRecipe.ingredients
            .Find(x => x.item.itemID == item.itemID);

        return ingredient != null ? ingredient.amount : 0;
    }

    public void ClearSlot(int index)
    {
        if (slots.ContainsKey(index))
            slots.Remove(index);
    }

    public bool IsRecipeComplete()
    {
        if (currentRecipe == null) return false;

        foreach (var ingredient in currentRecipe.ingredients)
        {
            bool found = false;

            foreach (var slot in slots.Values)
            {
                if (slot.item.itemID == ingredient.item.itemID &&
                    slot.amount >= ingredient.amount)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                return false;
        }

        return true;
    }
    public RecipeData_SO GetCurrentRecipe()
    {
        return currentRecipe;
    }
}