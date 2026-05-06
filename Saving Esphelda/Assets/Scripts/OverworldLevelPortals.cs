using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldLevelPortals : MonoBehaviour
{
    public string sceneToLoad;

    private void OnMouseDown()
    {
        Debug.Log($"Clicked on {gameObject.name}. Loading scene: {sceneToLoad}");
        if (SceneController.Instance != null)
        {
            SceneController.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
