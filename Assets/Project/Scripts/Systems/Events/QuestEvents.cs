using System;

public static class QuestEvents
{
    public static Action<ItemData_SO, int> OnItemCollected;
    public static Action<ItemData_SO, int> OnItemCrafted;
    public static Action<ItemData_SO, int> OnItemRefined;
}