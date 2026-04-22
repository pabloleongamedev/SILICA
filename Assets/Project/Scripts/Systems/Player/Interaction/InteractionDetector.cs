using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    public IInteractable CurrentInteractable { get; private set; }

    private void Update()
    {
        // 🔥 Limpieza de referencias destruidas
        interactables.RemoveAll(i => i == null);

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
        Debug.Log("Entró en trigger: " + other.name);

        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && !interactables.Contains(interactable))
        {
            interactables.Add(interactable);
            Debug.Log("Interactuable agregado");

            // 🔥 Suscribirse si es ItemPickup
            if (interactable is ItemPickup item)
            {
                item.OnPicked += HandleItemPicked;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && interactables.Contains(interactable))
        {
            interactables.Remove(interactable);
            Debug.Log("Interactuable removido");

            // 🔥 Desuscribirse si es ItemPickup
            if (interactable is ItemPickup item)
            {
                item.OnPicked -= HandleItemPicked;
            }
        }
    }

    // 🔥 Limpieza inmediata cuando el item se recoge
    private void HandleItemPicked(ItemPickup item)
    {
        if (interactables.Contains(item))
        {
            interactables.Remove(item);
            Debug.Log("Item removido por evento OnPicked");
        }
    }
    public string GetCurrentInteractionText()
    {
        if (CurrentInteractable == null) return string.Empty;

        return CurrentInteractable.GetInteractionText();
    }
}