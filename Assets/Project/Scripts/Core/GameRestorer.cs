using UnityEngine;
using System.Collections;

/// <summary>
/// GameRestorer: Se coloca en la escena de juego para restaurar el estado del jugador
/// cuando se carga una partida guardada.
/// 
/// USO:
/// 1. Crear un GameObject vacío en la escena de juego
/// 2. Agregar este script
/// 3. Se ejecutará automáticamente al cargar la escena
/// </summary>
public class GameRestorer : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private bool shouldRestorePosition = false;

    private void Start()
    {
        // Usar coroutine para asegurar que TODOS los Awake() y Start() se hayan ejecutado primero
        StartCoroutine(RestorePlayerStateDelayed());
    }

    private IEnumerator RestorePlayerStateDelayed()
    {
        // Esperar varios frames para que TODOS los sistemas se inicialicen
        yield return new WaitForSeconds(0.15f);

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[GameRestorer] GameManager no está disponible");
            yield break;
        }

        GameData gameData = GameManager.Instance.GetCurrentGameData();

        if (gameData == null)
        {
            Debug.LogWarning("[GameRestorer] No hay GameData para restaurar");
            yield break;
        }

        // Obtener posición y rotación a restaurar
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            Transform playerTransform = playerController.transform;
            targetPosition = gameData.playerData.GetPosition();
            targetRotation = gameData.playerData.GetRotation();
            
            Debug.Log($"[GameRestorer] Posición actual antes de restaurar: {playerTransform.position}");
            Debug.Log($"[GameRestorer] Posición guardada a restaurar: {targetPosition}");
            
            // Primera restauración
            RestorePositionAndRotation(playerTransform);
            
            // Marcar para verificaciones posteriores
            shouldRestorePosition = true;
            
            // Esperar un frame más y verificar
            yield return new WaitForSeconds(0.05f);
            
            // Segunda restauración (por si algo la cambió)
<<<<<<< HEAD
            RestorePositionAndRotation(playerTransform);
=======
           // RestorePositionAndRotation(playerTransform);
>>>>>>> 7ca46c4 (restore scripts interaction system)
            
            Debug.Log($"[GameRestorer] ✓ Posición final: {playerTransform.position}");
            Debug.Log($"[GameRestorer] ✓ Rotación final: {playerTransform.eulerAngles}");
        }
        else
        {
            Debug.LogError("[GameRestorer] ✗ PlayerController no encontrado en la escena");
        }
    }

    private void RestorePositionAndRotation(Transform playerTransform)
    {
        playerTransform.position = targetPosition;
        playerTransform.rotation = targetRotation;
        
        // Verificar que se aplicó
        float distance = Vector3.Distance(playerTransform.position, targetPosition);
        if (distance < 0.01f)
        {
            Debug.Log($"[GameRestorer] ✓ Posición aplicada correctamente: {playerTransform.position}");
        }
        else
        {
            Debug.LogWarning($"[GameRestorer] ✗ Posición no coincide. Distancia: {distance}");
        }
    }

    private void Update()
    {
        // Si estamos restaurando, asegurar que la posición se mantiene
        // (previene que otros sistemas la cambien)
        if (shouldRestorePosition)
        {
            PlayerController playerController = FindFirstObjectByType<PlayerController>();
            if (playerController != null)
            {
                Transform playerTransform = playerController.transform;
                float distance = Vector3.Distance(playerTransform.position, targetPosition);
                
                // Si la posición cambió, restaurarla
                if (distance > 0.1f)
                {
                    RestorePositionAndRotation(playerTransform);
                }
            }
            
            // Solo proteger durante los primeros 2 segundos
            Invoke(nameof(StopProtecting), 2f);
        }
    }

    private void StopProtecting()
    {
        shouldRestorePosition = false;
        Debug.Log("[GameRestorer] ✓ Protección de posición desactivada");
    }
}
