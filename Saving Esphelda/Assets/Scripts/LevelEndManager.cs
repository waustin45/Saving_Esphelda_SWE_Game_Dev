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

    void OnTriggerE2D(Collider2D collision)
    {
        Debug.Log("player hits");
        if (collision.gameObject.CompareTag("Player"))
        {
            var data = SaveManager.Load(PlayerPrefs.GetInt("SaveSlot"));
            data.keys = PlayerInventory.KeyCount;
            data.gems = PlayerInventory.GemCount;
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            data.nextLevel = currentSceneIndex + 1;
            data.levelsCompleted.Add(currentSceneIndex);
            SaveManager.Save(PlayerPrefs.GetInt("SaveSlot"), data);
        }
    }
}
