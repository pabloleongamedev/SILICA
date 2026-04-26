public class QuestTaskInstance
{
    private QuestTaskData_SO data;

    public int currentAmount { get; private set; }

    public bool IsComplete => currentAmount >= data.requiredAmount;

    public QuestTaskInstance(QuestTaskData_SO data)
    {
        this.data = data;
        currentAmount = 0;
    }

    public void Progress(ItemData_SO item, int amount, QuestTaskType type)
    {
        if (data.type != type)
            return;

        if (data.targetItem != item)
            return;

        currentAmount += amount;

        if (currentAmount > data.requiredAmount)
            currentAmount = data.requiredAmount;
    }
}