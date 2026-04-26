using System;
using UnityEngine;
public static class GameplayEvents
{
    public static System.Action<bool> OnInventoryToggle;
    public static System.Action<bool> OnCraftingToggle;
    public static System.Action<bool> OnChemistryToggle;
}