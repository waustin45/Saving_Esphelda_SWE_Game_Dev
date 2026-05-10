using System;
using System.Collections;
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

    private string previousScene;

    public void SetGameState(GameState newState)
    {
        if (newState == CurrentGameState)
            return;

        Debug.Log($"Setting GameState from {CurrentGameState} to {newState}");

        if (newState == GameState.Complete)
        {
            HandleCompleteState();
        }

        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void LoadOverworldFromLevel()
    {
        SavePlayerInventory();
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Overworld");
    }

    private void HandleCompleteState()
    {
        SavePlayerInventory();
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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            StartCoroutine(SpawnPlayerWhenReady(scene.name));
        }
    }

    private IEnumerator SpawnPlayerWhenReady(string currentSceneName)
    {
        float timeout = 1f;
        float elapsed = 0f;
        GameObject player = null;

        while (player == null && elapsed < timeout)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) break;

            yield return null;
            elapsed += Time.deltaTime;
        }

        if (player == null)
        {
            Debug.LogWarning($"SceneController: Unable to find Player in scene '{currentSceneName}' after load.");
            yield break;
        }

        SetPlayerSpawnPoint(currentSceneName);
        SetGameState(GameState.Active);
    }

    private void SetPlayerSpawnPoint(string currentSceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        if (currentSceneName == "Overworld")
        {
            string safeScene = string.IsNullOrEmpty(previousScene)
    ? "Overworld_Default"
    : previousScene;

          //  Vector3 spawnPosition = GetOverworldSpawnPosition(safeScene);
          //  player.transform.position = spawnPosition;
          //  Debug.Log($"Spawned player at {spawnPosition} in Overworld coming from {previousScene}");
        }
        else
{
    GameObject spawn = GameObject.FindWithTag("PlayerSpawn");

    if (spawn != null)
    {
        player.transform.position = spawn.transform.position;
    }
    else
    {
        Debug.LogWarning("No spawn found → using fallback (0,0,0)");
        player.transform.position = Vector3.zero;
    }
}}

    public void SetPreviousScene(string sceneName)
    {
        previousScene = sceneName;
    }

   /** private Vector3 GetOverworldSpawnPosition(string fromLevel)
    {
        string tag;
        switch (fromLevel)
        {
            case "Tutorial_Scene":
                tag = "L1 Portal";
                break;
            case "Level_One":
                tag = "L2 Portal";
                break;
            case "Level_Two":
                tag = "L3 Portal";
                break;
            case "Level_Three":
                tag = "L3 Portal";
                break;
            default:
                Debug.LogWarning($"Unknown level source '{fromLevel}' for overworld spawn. Using default position.");
                return Vector3.zero;
        }

        GameObject spawnObj = GameObject.FindGameObjectWithTag(tag);
        if (spawnObj != null)
        {
            return spawnObj.transform.position;
        }

        Debug.LogWarning($"No overworld spawn object found with tag '{tag}'. Using default position.");
        return Vector3.zero;
    } **/

    void Start()
    {
        SetGameState(GameState.Active);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("Player has died!");
        //reload the current scene after a short delay to allow death animation/sound to play
        Invoke(nameof(ReloadCurrentScene), 2f);
    }

    private void ReloadCurrentScene()
    {
        previousScene = "";
        SavePlayerInventory();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {

    }

    private void SavePlayerInventory()
    {
        PlayerInventory playerInventory = UnityEngine.Object.FindFirstObjectByType<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.SaveInventory();
            Debug.Log("PlayerInventory saved before scene change.");
        }
        else
        {
            Debug.LogWarning("PlayerInventory not found. Could not save inventory before scene change.");
        }
    }

    public void ExitLevelToOverworld()
    {
        previousScene = SceneManager.GetActiveScene().name;

        Debug.Log("Exiting level early → Overworld");

        SceneManager.LoadScene("Overworld");
    }

}
