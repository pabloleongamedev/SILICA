using System.Collections.Generic;
using UnityEngine;

public class InventoryGridSystem
{
    private int width;
    private int height;

    private InventoryItem[,] grid;
    private List<InventoryItem> items = new List<InventoryItem>();

    public InventoryGridSystem(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new InventoryItem[width, height];
    }

    // 🔹 Verifica si hay espacio
    public bool CanPlaceItem(ItemData_SO data, int x, int y)
    {
        if (x + data.width > width || y + data.height > height)
            return false;

        for (int i = 0; i < data.width; i++)
        {
            for (int j = 0; j < data.height; j++)
            {
                if (grid[x + i, y + j] != null)
                    return false;
            }
        }

        return true;
    }

    // 🔹 Coloca item
    public bool PlaceItem(ItemData_SO data, int x, int y)
    {
        if (!CanPlaceItem(data, x, y))
            return false;

        InventoryItem item = new InventoryItem(data, x, y);

        for (int i = 0; i < data.width; i++)
        {
            for (int j = 0; j < data.height; j++)
            {
                grid[x + i, y + j] = item;
            }
        }

        items.Add(item);
        return true;
    }

    // 🔹 Remover item
    public void RemoveItem(InventoryItem item)
    {
        for (int i = 0; i < item.data.width; i++)
        {
            for (int j = 0; j < item.data.height; j++)
            {
                grid[item.x + i, item.y + j] = null;
            }
        }

        items.Remove(item);
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }
}