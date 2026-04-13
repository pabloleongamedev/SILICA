public class InventoryGrid
{
    private InventorySlot[,] grid;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public InventoryGrid(int width, int height)
    {
        Width = width;
        Height = height;

        grid = new InventorySlot[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new InventorySlot();
            }
        }
    }

    public bool TryAddItem(InventoryItemInstance item)
    {
        if (!TryFindFirstEmptySlot(out int x, out int y))
            return false; // invenario lleno

        grid[x, y].SetItem(item);
        return true;
    }
    public bool TryFindFirstEmptySlot(out int outX, out int outY)
    {
        for (int y = 0; y < Height; y++) // IMPORTANTE: orden tipo lectura
        {
            for (int x = 0; x < Width; x++)
            {
                if (grid[x, y].IsEmpty)
                {
                    outX = x;
                    outY = y;
                    return true;
                }
            }
        }

        outX = -1;
        outY = -1;
        return false;
    }

        public InventorySlot GetSlot(int x, int y)
        {
            return grid[x, y];
        }
    }