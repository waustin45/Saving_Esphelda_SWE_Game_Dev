using UnityEngine;
using System.IO;
using System.Collections.Generic;
[System.Serializable]
public class SaveData
{
    public int keys;
    public int gems;
    public int nextLevel;
    public List<int> levelsCompleted;
}

public static class SaveManager
{
    private static string SavePath(int slot) =>
        Application.persistentDataPath + "/save_slot_" + slot + ".json";

    public static void Save(int slot, SaveData data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath(slot), json);
        Debug.Log("Saved to slot " + slot + " at " + SavePath(slot));
    }
    public static SaveData Load(int slot)
    {
        string path = SavePath(slot);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Save file empty, creating new.");
                return CreateNewSave();
            }

            return JsonUtility.FromJson<SaveData>(json);
        }

        return CreateNewSave();
    }
    // ---- Check if slot has a save ----
    public static bool SlotExists(int slot)
    {
        return File.Exists(SavePath(slot));
    }

    // ---- Delete a slot ----
    public static void DeleteSlot(int slot)
    {
        string path = SavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted slot " + slot);
        }
    }

    // ---- Fresh save defaults ----
    public static SaveData CreateNewSave()
{
    SaveData data = new SaveData();
    data.nextLevel = 1;
    data.levelsCompleted = new List<int>();
    return data;
}
}
