using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldLevelPortals : MonoBehaviour
{
    public string sceneToLoad;

    private void OnMouseDown()
    {
        Debug.Log($"Clicked on {gameObject.name}. Loading scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
