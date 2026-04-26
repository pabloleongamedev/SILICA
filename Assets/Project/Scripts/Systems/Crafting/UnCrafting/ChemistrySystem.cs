using System.Linq;
using UnityEngine;

public class ChemistrySystem
{
    public bool CanSeparate(
        CompoundDefinition_SO compound,
        SeparationMethod_SO method,
        IInventoryReadModel read)
    {
        if (compound == null || method == null)
            return false;

        if (compound.requiredMethod != method)
            return false;

        int available = GetAmount(read, compound.inputItem);

        if (available <= 0)
            return false;

        var remove = new[]
        {
            (compound.inputItem, 1)
        };

        var add = compound.outputs
            .Select(o => (o.item, o.amount))
            .ToArray();

        return read.CanProcessBatch(remove, add);
    }

    public bool Execute(
        CompoundDefinition_SO compound,
        SeparationMethod_SO method,
        IInventoryReadModel read,
        IInventoryWriteModel write)
    {
        if (!CanSeparate(compound, method, read))
            return false;

        // 🔥 CONSUMIR
        write.RemoveItem(compound.inputItem, 1);

        // 🔥 PRODUCIR
        foreach (var output in compound.outputs)
        {
            write.AddItem(output.item, output.amount);
        }

        return true;
    }

    // =========================
    // UTILIDAD INTERNA
    // =========================
    private int GetAmount(IInventoryReadModel read, ItemData_SO item)
    {
        int total = 0;

        for (int i = 0; i < read.Capacity; i++)
        {
            var slot = read.GetItem(i);

            if (slot == null)
                continue;

            if (slot.Data == item)
                total += slot.Quantity;
        }

        return total;
    }
}