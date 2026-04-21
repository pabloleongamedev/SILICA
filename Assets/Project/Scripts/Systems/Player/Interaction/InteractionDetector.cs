using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    public IInteractable CurrentInteractable { get; private set; }

    private void Update()
    {
        UpdateCurrent();
    }

    private void UpdateCurrent()
    {
        // simple: toma el último (puedes mejorar esto luego por distancia)
        if (interactables.Count > 0)
        {
            CurrentInteractable = interactables[interactables.Count - 1];
        }
        else
        {
            CurrentInteractable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactables.Remove(interactable);
        }
    }
}