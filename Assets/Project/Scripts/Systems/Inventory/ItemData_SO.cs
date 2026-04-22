using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Item")]
public class ItemData_SO : ScriptableObject
{
    public string itemID;
    public string displayName;
    public Sprite icon;
<<<<<<< HEAD
=======
    public int maxStack = 99;
>>>>>>> 7ca46c4 (restore scripts interaction system)

    [TextArea]
    public string description;

<<<<<<< HEAD
    // Para futuro crafting
    public int maxStack = 1;
    public int cantidad = 1;
=======
>>>>>>> 7ca46c4 (restore scripts interaction system)
}