using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class QuestData_SO : ScriptableObject
{
    public string questName;
    public QuestTaskData_SO[] tasks;
}