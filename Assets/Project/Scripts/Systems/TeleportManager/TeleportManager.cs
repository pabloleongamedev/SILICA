using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportManager : MonoBehaviour
{
    public InputActionReference interactAction;
    private void OnEnable() => interactAction.action.Enable();
    private void OnDisable() => interactAction.action.Disable();

    void Update()
    {
        if (interactAction.action.triggered)
        {
            Debug.Log($"Log: Tecla E Puldsada");
        }
    }

    void Start()
    {
        // Esto imprimirá cada acción y la tecla que tiene asignada
        foreach (var action in interactAction.action.actionMap.actions)
        {
            foreach (var binding in action.bindings)
            {
                Debug.Log($"Acción: {action.name} | Tecla: {binding.path}");
            }
        }
    }

}







