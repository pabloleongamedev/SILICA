using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Informacion de la Partida")]
    public string slotID = "1";
    public string lastSaveTime = "";
    public int playTimeSeconds = 0;

    [Header("Datos del Jugador")]
    public PlayerSaveData playerData = new PlayerSaveData();

    [Header("Inventario")]
    public List<InventorySaveData> inventoryItems = new List<InventorySaveData>();

    [Header("Progreso")]
    public string currentScene = "MainMenu";
    public List<string> scannedElements = new List<string>();

    [Header("Estado del Mundo")]
    public List<string> collectedItems = new List<string>();
    public List<string> destroyedObjects = new List<string>();

    public static GameData CreateNewGame(string slotID)
    {
        return new GameData
        {
            slotID = slotID,
            lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            playTimeSeconds = 0,
            playerData = PlayerSaveData.CreateDefault(),
            currentScene = "Pablo_TestMechanics",
            inventoryItems = new List<InventorySaveData>(),
            scannedElements = new List<string>(),
            collectedItems = new List<string>(),
            destroyedObjects = new List<string>()
        };
    }

    public void UpdatePlayTime(int deltaSeconds)
    {
        playTimeSeconds += deltaSeconds;
        lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string GetPlayTimeFormatted()
    {
        int hours = playTimeSeconds / 3600;
        int minutes = (playTimeSeconds % 3600) / 60;
        return $"{hours}h {minutes}m";
    }
}
