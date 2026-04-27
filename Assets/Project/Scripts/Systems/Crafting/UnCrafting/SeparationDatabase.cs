using UnityEngine;

[CreateAssetMenu(menuName = "Chemistry/Database")]
public class SeparationDatabase_SO : ScriptableObject
{
    public CompoundDefinition_SO[] compounds;

    public CompoundDefinition_SO Get(ItemData_SO item)
    {
        foreach (var c in compounds)
        {
            if (c.inputItem == item)
                return c;
        }

        return null;
    }
}