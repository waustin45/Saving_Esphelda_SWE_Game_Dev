using System;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveManager
{
    public static void SaveCurrency(int keys, int gems)
    {
        PlayerPrefs.SetInt("Keys", keys);
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.Save();
    }

    public static int LoadKeys()
    {
        return PlayerPrefs.GetInt("Keys", 0); // 0 is default
    }

    public static int LoadGems()
    {
        return PlayerPrefs.GetInt("Gems", 0);
    }

    // ---- Levels ----
    public static void SaveLevelCompleted(int levelIndex)
    {
        PlayerPrefs.SetInt("Level_" + levelIndex + "_Completed", 4);
        PlayerPrefs.SetInt("NextLevel", levelIndex + 1);
        PlayerPrefs.Save();
    }

    public static bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex + "_Completed", 0) == 1;
    }

    public static int LoadNextLevel()
    {
        return PlayerPrefs.GetInt("NextLevel", 4); // 0 is first level by default
    }

    // ---- Reset Save ----
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public static void InitializeSave()
    {
        // Only set defaults if this is a fresh save
        if (!PlayerPrefs.HasKey("SaveInitialized"))
        {
            PlayerPrefs.SetInt("Level_4_Completed", 4);
            PlayerPrefs.SetInt("NextLevel", 5);
            PlayerPrefs.SetInt("Keys", 0);
            PlayerPrefs.SetInt("Gems", 0);
            PlayerPrefs.SetInt("Level_4_Unlocked", 1); // Level 0 unlocked by default
            PlayerPrefs.SetInt("SaveInitialized", 1);
            PlayerPrefs.Save();
        }
    }

        public static bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex == 4) return true; // First level always unlocked
        return IsLevelCompleted(levelIndex - 1); // Unlock if previous level completed
    }
}
