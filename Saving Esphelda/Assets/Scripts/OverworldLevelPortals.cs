using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldLevelPortals : MonoBehaviour
{
    private SaveData data;
    public string sceneToLoad;
    [SerializeField] int index;




    void Start()
    {
        data = SaveManager.Load(PlayerPrefs.GetInt("SaveSlot"));
        Debug.Log(data.nextLevel);


        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (data.levelsCompleted.Contains(index) || data.nextLevel == index)
        {
            sr.color = new Color32(47, 0, 111, 255); // unlocked color
        }
        else
        {
            sr.color = new Color32(255, 57, 65, 255); // locked color
        }
    }
    private void OnMouseDown()
    {
        if(!data.levelsCompleted.Contains(index) && data.nextLevel != index) return;
        Debug.Log($"Clicked on {gameObject.name}. Loading scene: {sceneToLoad}");
        if (SceneController.Instance != null)
        {
            SceneController.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadScene(sceneToLoad);
    }



}
