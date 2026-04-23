using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    public IInteractable CurrentInteractable { get; private set; }

    public System.Action<IInteractable> OnInteractableChanged;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float maxAngle = 60f;

    private float cosAngleThreshold;

    private void Awake()
    {
        if (playerTransform == null)
            playerTransform = transform;

        cosAngleThreshold = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
    }

    private void Update()
    {
<<<<<<< HEAD
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
=======
        CleanInvalidInteractables();
        EvaluateBestInteractable();
>>>>>>> 52f1bdd (Sistemas de Inventario, Crafteo e Interaccion completos)
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
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
=======
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (!interactables.Contains(interactable))
                interactables.Add(interactable);
>>>>>>> 52f1bdd (Sistemas de Inventario, Crafteo e Interaccion completos)
        }
    }

    private void OnTriggerExit(Collider other)
    {
<<<<<<< HEAD
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
=======
        if (other.TryGetComponent<IInteractable>(out var interactable))
>>>>>>> 52f1bdd (Sistemas de Inventario, Crafteo e Interaccion completos)
        {
            interactables.Remove(interactable);
        }
    }

    private void CleanInvalidInteractables()
    {
        interactables.RemoveAll(i =>
        {
            if (i == null) return true;

            var mb = i as MonoBehaviour;
            return mb == null || !mb.gameObject.activeInHierarchy;
        });
    }

    private void EvaluateBestInteractable()
    {
        IInteractable best = null;
        float bestDistance = float.MaxValue;

        foreach (var interactable in interactables)
        {
            var mb = interactable as MonoBehaviour;
            if (mb == null) continue;

            Vector3 targetPos = mb.transform.position;

            float distance = Vector3.Distance(playerTransform.position, targetPos);
            if (distance > maxDistance) continue;

            if (cameraTransform != null)
            {
                Vector3 dir = (targetPos - cameraTransform.position).normalized;
                float dot = Vector3.Dot(cameraTransform.forward, dir);

                if (dot < cosAngleThreshold) continue;
            }

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = interactable;
            }
        }

        if (CurrentInteractable != best)
        {
            CurrentInteractable = best;
            OnInteractableChanged?.Invoke(CurrentInteractable);
        }
    }
<<<<<<< HEAD
    public string GetCurrentInteractionText()
    {
        if (CurrentInteractable == null) return string.Empty;

        return CurrentInteractable.GetInteractionText();
    }
>>>>>>> 7ca46c4 (restore scripts interaction system)
=======
>>>>>>> 52f1bdd (Sistemas de Inventario, Crafteo e Interaccion completos)
}