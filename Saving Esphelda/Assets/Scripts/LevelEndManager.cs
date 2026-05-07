using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManager : MonoBehaviour
{
    [SerializeField] PlayerInventory PlayerInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("player hits");
        if (collision.gameObject.CompareTag("Player"))
        {
            var slot = PlayerPrefs.GetInt("SaveSlot");
            Debug.Log(slot + " this is save from finishing level");
            var data = SaveManager.Load(slot);
            if (data == null)
            {
                Debug.LogError("SaveManager.Load returned null for slot: " + slot);
                return;
            }
            data.keys += PlayerInventory.KeyCount;
            data.gems += PlayerInventory.GemCount;
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Tutorial_Level") currentSceneIndex = 3;
            data.nextLevel = currentSceneIndex + 1;
            if (!data.levelsCompleted.Contains(currentSceneIndex))
            {
                data.levelsCompleted.Add(currentSceneIndex);
            }
            SaveManager.Save(slot, data);

            if (SceneController.Instance.CurrentGameState != SceneController.GameState.Active) return;
            Debug.Log("Checkpoint detected!");
            SceneController.Instance.SetGameState(SceneController.GameState.Complete);
        }

    }
}
