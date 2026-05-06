using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        //reset inventory at game start (tutorial scene), load from prefs on level transitions
        string currentScene = SceneManager.GetActiveScene().name;
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
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        KeyCount = Mathf.Clamp(PlayerPrefs.GetInt(KeyCountPref, 0), 0, MaxKeys);
        GemCount = Mathf.Max(PlayerPrefs.GetInt(GemCountPref, 0), 0);
    }

    private void ResetInventoryData()
    {
        KeyCount = 0;
        GemCount = 0;
        SaveInventory();
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

            if (filledChild != null && emptyChild != null)
            {
                bool isFilled = i < KeyCount;
                filledChild.gameObject.SetActive(isFilled);
                emptyChild.gameObject.SetActive(!isFilled);
            }
        }
    }
}
