using System.Linq;

public class QuestInstance
{
    private QuestData_SO data;

    private QuestTaskInstance[] tasks;

    public bool IsComplete => tasks.All(t => t.IsComplete);

    public QuestInstance(QuestData_SO data)
    {
        this.data = data;

        tasks = data.tasks
            .Select(t => new QuestTaskInstance(t))
            .ToArray();
    }

    public void Progress(ItemData_SO item, int amount, QuestTaskType type)
    {
        foreach (var task in tasks)
        {
            task.Progress(item, amount, type);
        }
    }
}