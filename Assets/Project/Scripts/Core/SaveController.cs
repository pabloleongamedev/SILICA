using System.IO;
using UnityEngine;

public class SaveController
{
    private readonly string saveFolderPath;

    public SaveController()
    {
        saveFolderPath = Path.Combine(Application.persistentDataPath, "Saves");

        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);
    }

    public string GetSaveFilePath(string slotID)
    {
        return Path.Combine(saveFolderPath, $"save_{slotID}.json");
    }

    public bool HasSaveFile(string slotID)
    {
        return File.Exists(GetSaveFilePath(slotID));
    }

    public void SaveGame(GameData gameData, string slotID)
    {
        if (gameData == null)
            return;

        gameData.slotID = slotID;
        gameData.lastSaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string filePath = GetSaveFilePath(slotID);

        try
        {
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveController] Error al guardar partida: {e.Message}");
        }
    }

    public GameData LoadGame(string slotID)
    {
        string filePath = GetSaveFilePath(slotID);

        if (!File.Exists(filePath))
            return null;

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveController] Error al cargar partida: {e.Message}");
            return null;
        }
    }

    public void DeleteSave(string slotID)
    {
        string filePath = GetSaveFilePath(slotID);

        if (!File.Exists(filePath))
            return;

        try
        {
            File.Delete(filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveController] Error al eliminar partida: {e.Message}");
        }
    }

    public SaveInfo GetSaveInfo(string slotID)
    {
        if (!HasSaveFile(slotID))
            return null;

        GameData data = LoadGame(slotID);
        if (data == null)
            return null;

        return new SaveInfo
        {
            slotID = slotID,
            scene = data.currentScene,
            lastSaveTime = data.lastSaveTime,
            playTime = data.GetPlayTimeFormatted(),
            playTimeSeconds = data.playTimeSeconds
        };
    }

    public SaveInfo[] GetAllSaveInfos()
    {
        SaveInfo[] infos = new SaveInfo[3];

        for (int i = 1; i <= 3; i++)
            infos[i - 1] = GetSaveInfo(i.ToString());

        return infos;
    }
}
