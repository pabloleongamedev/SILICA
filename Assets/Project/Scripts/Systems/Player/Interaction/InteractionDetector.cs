using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    public IInteractable CurrentInteractable { get; private set; }

    private void Update()
    {
        if (interactables.Count > 0)
        {
            CurrentInteractable = interactables[interactables.Count - 1];
        }
        else
        {
            CurrentInteractable = null;
        }

        Debug.Log("Current: " + CurrentInteractable);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró en trigger: " + other.name);

        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            if (!interactables.Contains(interactable))
            {
                interactables.Add(interactable);
                Debug.Log("Interactuable agregado");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            if (interactables.Contains(interactable))
            {
                interactables.Remove(interactable);
                Debug.Log("Interactuable removido");
            }
        }
    }
}