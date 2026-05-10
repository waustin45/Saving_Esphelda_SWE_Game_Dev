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

        if (data.levelsCompleted.Contains(index))
        {
            //level completed green
            sr.color = new Color32(57, 255, 65, 255);
        }
        else if (index == 1)
        {
            //level 1 always unlocked purple
            sr.color = new Color32(47, 0, 111, 255);
        }
        else if (index == data.nextLevel && index <= data.nextLevel)
        {
            //level unlocked → purple
            sr.color = new Color32(47, 0, 111, 255);
        }
        else
        {
            //level locked red
            sr.color = new Color32(255, 57, 65, 255);
        }
    }


    private void OnMouseDown()
    {
        if (!data.levelsCompleted.Contains(index) && data.nextLevel != index) return;
        Debug.Log($"Clicked on {gameObject.name}. Loading scene: {sceneToLoad}");
        if (SceneController.Instance != null)
        {
            SceneController.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadScene(sceneToLoad);
    }



}
