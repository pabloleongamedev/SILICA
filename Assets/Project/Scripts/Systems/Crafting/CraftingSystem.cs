using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem
{
    private RecipeData_SO currentRecipe;

    // 🔥 Estado interno del crafting (NO inventario real)
    private Dictionary<int, (ItemData_SO item, int amount)> slots = new();

    // =========================
    // SET RECIPE
    // =========================
    public void SetRecipe(RecipeData_SO recipe)
    {
        currentRecipe = recipe;
        slots.Clear();
    }

    // =========================
    // PLACE ITEM (CLAVE)
    // =========================
    public bool TryPlaceItem(int slotIndex, ItemData_SO item, IInventoryReadModel read, IInventoryWriteModel write)
    {
        if (slots.ContainsKey(slotIndex))
        {
            Debug.Log("Slot ya ocupado");
            return false;
        }
        if (currentRecipe == null)
            return false;

        // validar que pertenece a la receta
        var ingredient = currentRecipe.ingredients
            .Find(x => x.item.itemID == item.itemID);

        if (ingredient == null)
        {
            Debug.Log("Item no pertenece a la receta");
            return false;
        }

        // cuánto ya hay colocado
        int current = GetCurrentAmount(item);
        int required = ingredient.amount;

        if (current >= required)
        {
            Debug.Log("Ingrediente ya completo");
            return false;
        }

        // cuánto falta
        int remaining = required - current;

        // validar inventario (READ)
        int available = read.GetAmount(item);

        if (available < remaining)
        {
            Debug.Log("No hay suficientes items en inventario");
            return false;
        }

        // consumir inventario (WRITE)
        write.RemoveItem(item, remaining);

        // guardar en slot
        slots[slotIndex] = (item, remaining);

        return true;
    }
    public void ClearAllNoReturn()
    {
        slots.Clear();
    }

    // =========================
    // GET CURRENT AMOUNT
    // =========================
    public int GetCurrentAmount(ItemData_SO item)
    {
        int total = 0;

        foreach (var s in slots.Values)
        {
            if (s.item == item)
                total += s.amount;
        }

        return total;
    }

    // =========================
    // CLEAR SLOT (DEVOLVER)
    // =========================
    public void ClearSlot(int index, IInventoryWriteModel write)
    {
        if (!slots.ContainsKey(index))
            return;

        var data = slots[index];

        // 🔥 devolver items al inventario
        write.AddItem(data.item, data.amount);

        slots.Remove(index);
    }

    // =========================
    // CLEAR ALL
    // =========================
    public void ClearAll(IInventoryWriteModel write)
    {
        foreach (var slot in slots.Values)
        {
            write.AddItem(slot.item, slot.amount);
        }

        slots.Clear();
    }

    // =========================
    // VALIDACIÓN
    // =========================
    public bool IsRecipeComplete()
    {
        if (currentRecipe == null)
            return false;

        foreach (var ing in currentRecipe.ingredients)
        {
            if (GetCurrentAmount(ing.item) < ing.amount)
                return false;
        }

        return true;
    }

    // =========================
    // BATCH REMOVE (para validación)
    // =========================
    public (ItemData_SO item, int amount)[] BuildRemoveBatch()
    {
        if (currentRecipe == null)
            return new (ItemData_SO, int)[0];

        var list = new List<(ItemData_SO, int)>();

        foreach (var ing in currentRecipe.ingredients)
        {
            list.Add((ing.item, ing.amount));
        }

        return list.ToArray();
    }

    // =========================
    // ACCESS
    // =========================
    public RecipeData_SO GetCurrentRecipe()
    {
        return currentRecipe;
    }
}