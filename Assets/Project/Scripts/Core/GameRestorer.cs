using UnityEngine;

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
    private void Awake()
    {
        GameData gameData = GameManager.Instance?.GetCurrentGameData();

        if (gameData == null)
        {
            Debug.LogWarning("[GameRestorer] No hay GameData para restaurar");
            return;
        }

        // Restaurar posición y rotación del jugador
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Transform playerTransform = playerController.transform;
            playerTransform.position = gameData.playerData.GetPosition();
            playerTransform.rotation = gameData.playerData.GetRotation();

            Debug.Log($"[GameRestorer] Jugador restaurado a posición: {playerTransform.position}");
        }
        else
        {
            Debug.LogWarning("[GameRestorer] PlayerController no encontrado en la escena");
        }

        // Aquí se podrían restaurar otros sistemas:
        // - Inventario
        // - Salud
        // - Habilidades
        // - Estado del mundo
    }
}
