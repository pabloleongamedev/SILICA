using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    private List<QuestInstance> activeQuests = new();



    // =========================
    // PRUEBA DE FUNCIONAMIENTO -->BORRAR
    // =========================
    [SerializeField] private QuestData_SO testQuest;

    private void Start()
    {
        if (testQuest != null)
        {
            AddQuest(testQuest);
            Debug.Log("Quest cargada: " + testQuest.name);
        }
    }
    // =========================
    // INIT
    // =========================
    public void AddQuest(QuestData_SO questData)
    {
        activeQuests.Add(new QuestInstance(questData));
    }

    // =========================
    // PROGRESS
    // =========================
    public void Progress(ItemData_SO item, int amount, QuestTaskType type)
    {
        foreach (var quest in activeQuests)
        {
            quest.Progress(item, amount, type);

            if (quest.IsComplete)
            {
                Debug.Log("MISIÓN COMPLETADA");
            }
        }
    }
    private void OnEnable()
    {
        QuestEvents.OnItemCollected += OnCollected;
        QuestEvents.OnItemCrafted += OnCrafted;
        QuestEvents.OnItemRefined += OnRefined;
    }

    private void OnDisable()
    {
        QuestEvents.OnItemCollected -= OnCollected;
        QuestEvents.OnItemCrafted -= OnCrafted;
        QuestEvents.OnItemRefined -= OnRefined;
    }

    private void OnCollected(ItemData_SO item, int amount)
    {
        Progress(item, amount, QuestTaskType.CollectItem);
    }

    private void OnCrafted(ItemData_SO item, int amount)
    {
        Progress(item, amount, QuestTaskType.CraftItem);
    }

    private void OnRefined(ItemData_SO item, int amount)
    {
        Progress(item, amount, QuestTaskType.RefineItem);
    }
}