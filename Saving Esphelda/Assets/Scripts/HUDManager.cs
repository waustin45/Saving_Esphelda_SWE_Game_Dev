using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI levelNameText;
    
    private Dictionary<string, string> sceneDisplayNames = new Dictionary<string, string>
    {
        { "Tutorial_Scene", "Tutorial" },
        { "Level_One", "Level 1" },
        { "Level_Two", "Level 2" },
        { "Level_Three", "Level 3" },
        { "Overworld", "Overworld" }
    };

    private void Start()
    {
        UpdateLevelName();
    }

    private void UpdateLevelName()
    {
       string currentScene = SceneManager.GetActiveScene().name;
        
        if (sceneDisplayNames.TryGetValue(currentScene, out string displayName))
        {
            levelNameText.text = displayName;
        }
        else
        {
            //fallback: use scene name directly
            levelNameText.text = currentScene;
        }
        
        Debug.Log($"HUDManager: Updated level name to '{levelNameText.text}' for scene '{currentScene}'");
    }
}
