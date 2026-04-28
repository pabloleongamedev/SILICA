using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportManager : MonoBehaviour
{
    public InputActionReference teleportAction;
    public Transform spaceShipHall;
    public Transform exteriorMap;
    public GameObject player;
    public GameObject visualPrompt;
    private static bool onSpaceShip = false;
    private bool isNearDoor = false;

    private void OnEnable()
    {
        teleportAction.action.Enable();
    }
    private void OnDisable()
    {
        teleportAction.action.Disable();
    }


    void Start()
    {
        if (visualPrompt != null)
        {
            visualPrompt.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isNearDoor && teleportAction.action.WasPressedThisFrame())
        {
            Debug.Log("¡BOTÓN PRESIONADO! Iniciando teletransporte...");
            teleportPlayer();
        }
    }


    private void teleportPlayer()
    {
        Vector3 endPoint = onSpaceShip ? exteriorMap.position : spaceShipHall.position;
        CharacterController controller = player.GetComponent<CharacterController>();
        Rigidbody rigidBody = player.GetComponent<Rigidbody>();
        if (controller != null)
        {
            controller.enabled = false;
        }
        if (rigidBody != null)
        {
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.isKinematic = true;
        }
        player.transform.position = endPoint;
        Physics.SyncTransforms();
        if (controller != null) controller.enabled = true;

        if (rigidBody != null)
        {
            rigidBody.isKinematic = false;
        }
        onSpaceShip = !onSpaceShip;
        isNearDoor = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.CompareTag("Player"))
        {
            isNearDoor = true;
            Debug.Log("Tecla E");
            if (visualPrompt != null)
            {
                visualPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDoor = false;
            if (visualPrompt != null)
            {
                visualPrompt.SetActive(false);
            }
        }
    }

}







