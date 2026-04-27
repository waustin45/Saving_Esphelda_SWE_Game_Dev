using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        SceneManager.LoadScene("Tutorial_Scene");
    }
    public void LoadGameMap()
    {
        SceneManager.LoadScene("Overworld");
    }
}
