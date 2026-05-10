using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Overworld");
    }
    public void LoadSaveSlotScene()
    {
        SceneManager.LoadScene("SaveSlots");

    }
}
