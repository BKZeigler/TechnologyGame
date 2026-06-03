using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saved to: " + SavePath);
    }

    public static GameSaveData Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);

        Debug.Log("Save file deleted at: " + SavePath);
    }
}