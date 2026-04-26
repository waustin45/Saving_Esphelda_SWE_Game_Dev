using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController Instance;

    public enum SceneName
    {
        MainMenu,
        Tutorial,
        Level1,
        Level2,
        Level3,
        GameOver
    }

    public enum GameState
    {
        Loading,
        Active,
        Paused,
        Complete,
        GameOver
    }

    public GameState CurrentGameState { get; private set; }
    public static Action<GameState> OnGameStateChanged;

    public void SetGameState(GameState newState)
    {
        if (newState == CurrentGameState)
            return;

        Debug.Log($"Setting GameState from {CurrentGameState} to {newState}");

        //handle state transitions based on new state, not the current one
        if (newState == GameState.Complete)
        {
            HandleCompleteState();
        }

        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }


    private void HandleCompleteState()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = index + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Loading next scene: {nextIndex}");
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No more scenes to load. Returning to main menu.");
            SceneManager.LoadScene(0);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetGameState(GameState.Active);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
