using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldLevelPortals : MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField] int index;

    private void OnMouseDown()
    {
        if(!SaveManager.IsLevelUnlocked(index)) return;
        Debug.Log($"Clicked on {gameObject.name}. Loading scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }

    void Awake()
    {
        SaveManager.InitializeSave();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (SaveManager.IsLevelUnlocked(index))
        {
            sr.color = new Color32(47, 0, 111, 255); // unlocked color
        }
        else
        {
            sr.color = new Color32(255, 57, 65, 255); // locked color
        }
        Debug.Log(SaveManager.IsLevelCompleted(index) ? $"Level completed {index} | {SaveManager.LoadNextLevel()}" : $"Level not completed {index} | {SaveManager.LoadNextLevel()}");
    }

}
