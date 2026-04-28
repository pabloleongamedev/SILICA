using UnityEngine;

public class TeleportManager : MonoBehaviour, IInteractable
{
    [Header("Targets")]
    [SerializeField] private Transform spaceShipHall;
    [SerializeField] private Transform exteriorMap;
    [SerializeField] private GameObject player;

    [Header("Refs")]
    [SerializeField] private QuestSystem questSystem;
    private bool canExit = false;

    // =========================================================
    public void Interact(InteractionContext context)
    {
        if (!canExit)
        {
            Notify("Debes completar la segunda misión", NotificationType.Warning);
            return;
        }

        TeleportPlayer(player);
    }

    public string GetInteractionText()
    {
        return "Presiona E para abrir la nave";
    }

    // =========================================================
    // 🔥 VALIDACIÓN DE MISIONES
    // =========================================================
    

    private void OnEnable()
    {
        QuestSystem.OnQuestCompleted += HandleQuestCompleted;
    }

    private void OnDisable()
    {
        QuestSystem.OnQuestCompleted -= HandleQuestCompleted;
    }

    private void HandleQuestCompleted(int questIndex)
    {
        if (questIndex >= 1)
        {
            canExit = true;
        }
    }

    // =========================================================
    private void TeleportPlayer(GameObject player)
    {
        Debug.Log("[Teleport] Ejecutando teletransporte");

        Vector3 playerPos = player.transform.position;

        float distToShip = Vector3.Distance(playerPos, spaceShipHall.position);
        float distToExterior = Vector3.Distance(playerPos, exteriorMap.position);

        Vector3 endPoint = distToShip < distToExterior
            ? exteriorMap.position
            : spaceShipHall.position;

        var controller = player.GetComponent<CharacterController>();
        var rigidBody = player.GetComponent<Rigidbody>();

        if (controller != null)
            controller.enabled = false;

        if (rigidBody != null)
        {
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.isKinematic = true;
        }

        player.transform.position = endPoint;

        Physics.SyncTransforms();

        if (controller != null)
            controller.enabled = true;

        if (rigidBody != null)
            rigidBody.isKinematic = false;
    }

    // =========================================================
    private void Notify(string msg, NotificationType type)
    {
        Debug.Log("[Teleport] " + msg);

        GameplayEvents.OnNotification?.Invoke(new NotificationData
        {
            message = msg,
            type = type
        });
    }
}