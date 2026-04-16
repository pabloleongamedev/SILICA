using System.Collections.Generic;

public class CraftingSystem
{
    private InventorySystem inventory;

    public CraftingSystem(InventorySystem inventory)
    {
        this.inventory = inventory;
    }

    public bool CanCraft(RecipeData_SO recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            int total = CountItem(ing.item);

            if (total < ing.amount)
                return false;
        }

        return true;
    }

    public bool Craft(RecipeData_SO recipe)
    {
        if (!CanCraft(recipe))
            return false;

        // 🔥 VALIDAR ESPACIO PRIMERO
        if (!inventory.AddItem(recipe.result, recipe.resultAmount))
            return false;

        // 🔥 CONSUMIR DESPUÉS
        foreach (var ing in recipe.ingredients)
        {
            RemoveItem(ing.item, ing.amount);
        }

        return true;
    }

    private int CountItem(ItemData_SO item)
    {
        int total = 0;
        var grid = inventory.GetGrid();

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (slot.IsEmpty)
                    continue;

                // 🔥 FIX
                if (slot.Item.Data.itemID == item.itemID)
                    total += slot.Item.Quantity;
            }
        }

        return total;
    }

    private void RemoveItem(ItemData_SO item, int amount)
    {
        var grid = inventory.GetGrid();
        int remaining = amount;

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var slot = grid.GetSlot(x, y);

                if (slot.IsEmpty)
                    continue;

                // 🔥 FIX
                if (slot.Item.Data.itemID != item.itemID)
                    continue;

                int removed = slot.Item.Remove(remaining);
                remaining -= removed;

                if (slot.Item.IsEmpty())
                    slot.Clear();

                if (remaining <= 0)
                    return;
            }
        }
    }
}