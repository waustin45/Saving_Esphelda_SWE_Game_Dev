using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class OverworldCameraController : MonoBehaviour
{
    [Header("Overworld Camera")]
    public string overworldSceneName = "Overworld";
    public Vector3 overworldPosition = new Vector3(0, 0, -1.44f);
    public bool lockInOverworld = true;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (!lockInOverworld)
            return;

        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"OverworldCameraController: Current scene '{currentScene}', checking against '{overworldSceneName}'");

        if (currentScene == overworldSceneName)
        {
            Debug.Log($"Setting camera position to {overworldPosition}");
            transform.position = overworldPosition;
        }
        else
        {
            Debug.Log("Allowing normal camera behavior in other scenes");
        }
    }
}