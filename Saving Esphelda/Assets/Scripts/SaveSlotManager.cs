using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotManager : MonoBehaviour
{
    [SerializeField] GameObject slotBtn1;
    [SerializeField] GameObject slotBtn2;
    [SerializeField] GameObject slotBtn3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (PlayerPrefs.HasKey("SaveSlot1"))
        // {
        //     var btn = slotBtn1.GetComponent<Button>();
        //     btn.enabled = false;
        // }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSaveSlot(int slot)
    {
        PlayerPrefs.SetInt("SaveSlot", slot);
        PlayerPrefs.SetInt("SaveSlot"+slot, 1);
        PlayerPrefs.Save();
        SaveManager.Save(slot, SaveManager.CreateNewSave());
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Title_Scene");
    }
}
