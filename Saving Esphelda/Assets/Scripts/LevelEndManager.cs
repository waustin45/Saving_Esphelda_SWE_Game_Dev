using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManager : MonoBehaviour
{

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
        Debug.Log("player hit");
        if (collision.gameObject.CompareTag("Player"))
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SaveManager.SaveLevelCompleted(currentSceneIndex);
        }
    }
}
