using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [Header("Canvas Objects")]
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject overworldButton;
    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Awake()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        pauseCanvas.SetActive(false);
        DisableOverworld();

    }

    void DisableOverworld()
    {
        if(SceneManager.GetActiveScene().name == "Overworld")
        {
            overworldButton.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                OpenPauseMenu();
                return;
            }

            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Opening pause menu
            pauseCanvas.SetActive(true);
            pauseMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            // Closing pause menu - reset everything
            pauseCanvas.SetActive(false);
            settingsPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void Resume()
    {
        isPaused = false;
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    // Called by Settings button
    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // Called by Back button in settings
    public void OpenPauseMenu()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        pauseCanvas.SetActive(true);
    }

    // Called by Restart button
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called by Overworld button
    public void GoToOverworld()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Overworld");
    }

    // Called by Main Menu button
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title_Scene");
    }

}
