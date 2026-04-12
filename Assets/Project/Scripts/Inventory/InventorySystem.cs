using System.Collections.Generic;

public class InventorySystem
{
    private List<ElementData> elements = new List<ElementData>();

    public void Add(ElementData data)
    {
        elements.Add(data);
    }

    public List<ElementData> GetAll()
    {
        return elements;
    }
}