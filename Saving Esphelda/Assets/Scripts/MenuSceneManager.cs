using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class MenuSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        
    }
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title_Scene");
    }
    public void LoadTutorialQuestionScene()
    {
        SceneManager.LoadScene("TutorialQuestionScene");
    }
    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial_Level");
    }
    public void LoadOverworld()
    {
        var slot = PlayerPrefs.GetInt("SaveSlot");
        var data = SaveManager.Load(slot);
        if (data == null)
        {
            Debug.LogError("SaveManager.Load returned null for slot: " + slot);
            return;
        }
        var nextLevel = data.nextLevel;

        if (data.levelsCompleted.Any())
        {
            data.nextLevel = Mathf.Max(data.nextLevel, data.levelsCompleted.Last() + 1);
        } else
        {
            data.nextLevel = 4;
        }


        
        SaveManager.Save(slot, data);
        SceneManager.LoadScene("Overworld");
    }
    public void LoadSaveSlotScene()
    {
        SceneManager.LoadScene("SaveSlots");

    }
}
