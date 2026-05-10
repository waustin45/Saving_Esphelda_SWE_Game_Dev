using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public int KeyCount = 0;
    public int MaxKeys = 3;
    public int GemCount = 0;

    public GameObject[] keySlots;
    public string emptyKey = "Empty";
    public string filledKey = "Filled";

    private const string KeyCountPref = "PlayerKeyCount";
    private const string GemCountPref = "PlayerGemCount";
    private static PlayerInventory instance;
    private static HashSet<string> collectedKeys = new HashSet<string>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"Inventory Awake in {currentScene}");

        //only reset on actual new game scenes
        if (currentScene == "Tutorial_Scene" || currentScene == "TutorialQuestionScene")
        {
            ResetInventoryData();
        }
        else
        {
            LoadInventory();
        }
    }

    void Start()
    {
        UpdateKeyHUD();
    }

    public void AddKey()
    {
        KeyCount = Mathf.Clamp(KeyCount + 1, 0, MaxKeys);
        Debug.Log($"AddKey() called! KeyCount: {KeyCount}");
        SaveInventory();
        UpdateKeyHUD();
    }

    public void AddGem(int amount)
    {
        GemCount = Mathf.Max(GemCount + amount, 0);
        SaveInventory();
    }

    public void SaveInventory()
    {
        PlayerPrefs.SetInt(KeyCountPref, KeyCount);
        PlayerPrefs.SetInt(GemCountPref, GemCount);

        //save to JSON
        int slot = PlayerPrefs.GetInt("SaveSlot", 1);
        var data = SaveManager.Load(slot);

        data.keys = KeyCount;
        data.gems = GemCount;

        SaveManager.Save(slot, data);

        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        int slot = PlayerPrefs.GetInt("SaveSlot", 1);
        var data = SaveManager.Load(slot);

        //prefer JSON if it exists
        if (data != null)
        {
            KeyCount = Mathf.Clamp(data.keys, 0, MaxKeys);
            GemCount = Mathf.Max(data.gems, 0);
        }
        else
        {
            KeyCount = Mathf.Clamp(PlayerPrefs.GetInt(KeyCountPref, 0), 0, MaxKeys);
            GemCount = Mathf.Max(PlayerPrefs.GetInt(GemCountPref, 0), 0);
        }

        Debug.Log($"LoadInventory: KeyCount={KeyCount}, GemCount={GemCount}");
    }
    public static bool HasCollectedKey(string id)
    {
        return collectedKeys.Contains(id);
    }

    public static void MarkKeyCollected(string id)
    {
        collectedKeys.Add(id);
    }

    private void ResetInventoryData()
    {
        KeyCount = 0;
        GemCount = 0;
        SaveInventory();
        Debug.Log($"ResetInventoryData: KeyCount set to {KeyCount}");
    }

    public void ResetInventory()
    {
        KeyCount = 0;
        GemCount = 0;
        SaveInventory();
        UpdateKeyHUD();
    }

    private void UpdateKeyHUD()
    {
        if (keySlots == null)
            return;

        for (int i = 0; i < keySlots.Length; i++)
        {
            if (keySlots[i] == null)
                continue;

            var filledChild = keySlots[i].transform.Find(filledKey);
            var emptyChild = keySlots[i].transform.Find(emptyKey);

            bool isFilled = i < KeyCount;

            if (filledChild != null)
                filledChild.gameObject.SetActive(isFilled);

            if (emptyChild != null)
                emptyChild.gameObject.SetActive(!isFilled);
        }
    }
}
