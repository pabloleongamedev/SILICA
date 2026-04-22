using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    public IInteractable CurrentInteractable { get; private set; }

    private void Update()
    {
<<<<<<< HEAD
=======
        // 🔥 Limpieza de referencias destruidas
        interactables.RemoveAll(i => i == null);

>>>>>>> 7ca46c4 (restore scripts interaction system)
        if (interactables.Count > 0)
        {
            CurrentInteractable = interactables[interactables.Count - 1];
        }
        else
        {
            CurrentInteractable = null;
        }
<<<<<<< HEAD

        Debug.Log("Current: " + CurrentInteractable);
=======
>>>>>>> 7ca46c4 (restore scripts interaction system)
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró en trigger: " + other.name);

        var interactable = other.GetComponentInParent<IInteractable>();

<<<<<<< HEAD
        if (interactable != null)
        {
            if (!interactables.Contains(interactable))
            {
                interactables.Add(interactable);
                Debug.Log("Interactuable agregado");
=======
        if (interactable != null && !interactables.Contains(interactable))
        {
            interactables.Add(interactable);
            Debug.Log("Interactuable agregado");

            // 🔥 Suscribirse si es ItemPickup
            if (interactable is ItemPickup item)
            {
                item.OnPicked += HandleItemPicked;
>>>>>>> 7ca46c4 (restore scripts interaction system)
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();

<<<<<<< HEAD
        if (interactable != null)
        {
            if (interactables.Contains(interactable))
            {
                interactables.Remove(interactable);
                Debug.Log("Interactuable removido");
            }
        }
    }
=======
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
>>>>>>> 7ca46c4 (restore scripts interaction system)
}