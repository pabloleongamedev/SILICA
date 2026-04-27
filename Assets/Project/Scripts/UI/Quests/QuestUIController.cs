using UnityEngine;
using TMPro;

public class QuestUIController : MonoBehaviour
{
    [Header("Mission")]
    [SerializeField] private Transform missionContent;
    [SerializeField] private GameObject missionPrefab;

    [Header("Tasks")]
    [SerializeField] private Transform taskContent;
    [SerializeField] private GameObject taskPrefab;

    private void OnEnable()
    {
        QuestEvents.OnQuestLoaded += BuildUI;
        QuestEvents.OnTaskUpdated += UpdateTask;

        var questSystem = FindFirstObjectByType<QuestSystem>();

        if (questSystem != null)
        {
            var quest = questSystem.GetCurrentQuest();

            if (quest != null)
            {
                BuildUI(quest);

                // RECONSTRUIR PROGRESO REAL
                for (int i = 0; i < quest.tasks.Count; i++)
                {
                    int current = questSystem.GetTaskProgress(i);
                    int required = quest.tasks[i].requiredAmount;

                    bool completed = current >= required;

                    UpdateTask(i, current, required, completed);
                }
            }
        }
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestLoaded -= BuildUI;
        QuestEvents.OnTaskUpdated -= UpdateTask;
    }

    // =========================================
    private void BuildUI(QuestData_SO quest)
    {
        Clear();

        // 🔥 MISION
        var missionGO = Instantiate(missionPrefab, missionContent);
        var missionText = missionGO.GetComponentInChildren<TMP_Text>();
        missionText.text = quest.questName;

        // 🔥 TAREAS
        foreach (var task in quest.tasks)
        {
            var go = Instantiate(taskPrefab, taskContent);
            var texts = go.GetComponentsInChildren<TMP_Text>();

            texts[0].text = task.description;
            texts[1].text = $"0 / {task.requiredAmount}";
        }
    }

    // =========================================
    private void UpdateTask(int index, int current, int required, bool completed)
    {
        var go = taskContent.GetChild(index);
        var texts = go.GetComponentsInChildren<TMP_Text>();

        texts[1].text = $"{current} / {required}";

        if (completed)
        {
            texts[0].color = Color.gray;
            texts[1].color = Color.gray;
        }
    }

    private void Clear()
    {
        foreach (Transform child in missionContent)
            Destroy(child.gameObject);

        foreach (Transform child in taskContent)
            Destroy(child.gameObject);
    }
}