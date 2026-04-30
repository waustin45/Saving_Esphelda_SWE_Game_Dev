using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotManager : MonoBehaviour
{
    [SerializeField] GameObject slotBtn1;
    [SerializeField] GameObject slotBtn2;
    [SerializeField] GameObject slotBtn3;
    [SerializeField] GameObject slot1Img;
    [SerializeField] GameObject slot2Img;
    [SerializeField] GameObject slot3Img;
    [SerializeField] Sprite xSprite;
    [SerializeField] Sprite plusSprite;

    private Color softGreen = new Color32(100, 200, 100, 255);
    private int lastSavedSlot = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("SaveSlot1") && SaveManager.SlotExists(1))
        {
            var btn = slotBtn1.GetComponent<Button>();
            var img = slot1Img.GetComponent<Image>();
            var text = slotBtn1.GetComponentInChildren<TMP_Text>();
            var btnImg = btn.GetComponent<Image>();
            text.text = "Slot 1 - Used";
            img.color = new Color(1f, 1f, 1f, 1f);
            
        }

        if (PlayerPrefs.HasKey("SaveSlot2")  && SaveManager.SlotExists(2))
        {
            var btn = slotBtn2.GetComponent<Button>();
            var img = slot2Img.GetComponent<Image>();
            var text = slotBtn2.GetComponentInChildren<TMP_Text>();
            var btnImg = btn.GetComponent<Image>();
           
            text.text = "Slot 2 - Used";
            img.color = new Color(1f, 1f, 1f, 1f);

        }
        if (PlayerPrefs.HasKey("SaveSlot3")  && SaveManager.SlotExists(3))
        {
            var btn = slotBtn3.GetComponent<Button>();
            var img = slot3Img.GetComponent<Image>();
            var text = slotBtn3.GetComponentInChildren<TMP_Text>();
            var btnImg = btn.GetComponent<Image>();
            
            text.text = "Slot 3 - Used";
            img.color = new Color(1f, 1f, 1f, 1f);

        }

    }



void Update()
{
    int currentSlot = PlayerPrefs.GetInt("SaveSlot", 0);
    
    // Only update if the slot has actually changed
    if (currentSlot != lastSavedSlot)
    {
        lastSavedSlot = currentSlot;
        CheckForSelected(currentSlot);
    }
}

void CheckForSelected(int slot)
{
    // Reset all buttons first
    slotBtn1.GetComponent<Button>().enabled = true;
    slotBtn2.GetComponent<Button>().enabled = true;
    slotBtn3.GetComponent<Button>().enabled = true;
    slotBtn1.GetComponentInChildren<Image>().color= Color.white;
    slotBtn2.GetComponentInChildren<Image>().color= Color.white;
    slotBtn3.GetComponentInChildren<Image>().color= Color.white;
    slotBtn1.GetComponentInChildren<TMP_Text>().text = "Slot 1";
    slotBtn2.GetComponentInChildren<TMP_Text>().text = "Slot 2";
    slotBtn3.GetComponentInChildren<TMP_Text>().text = "Slot 3";

    var slot1Save = SaveManager.SlotExists(1);
    var slot2Save = SaveManager.SlotExists(2);
    var slot3Save = SaveManager.SlotExists(3);
    if(!slot1Save) slot1Img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
    if(!slot2Save) slot2Img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
    if(!slot3Save) slot3Img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);

    // Then highlight the selected one
    switch (slot)
    {
        case 1:
            slotBtn1.GetComponentInChildren<Image>().color = softGreen;
            slotBtn1.GetComponent<Button>().enabled = false;
            if(slot1Save) slot1Img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if(slot1Save) slotBtn1.GetComponentInChildren<TMP_Text>().text = "Slot 1 - Used";
            break;
        case 2:
            slotBtn2.GetComponentInChildren<Image>().color = softGreen;
            slotBtn2.GetComponent<Button>().enabled = false;
            if(slot2Save) slot2Img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if(slot2Save) slotBtn2.GetComponentInChildren<TMP_Text>().text = "Slot 2 - Used";
            break;
        case 3:
            slotBtn3.GetComponentInChildren<Image>().color = softGreen;
            slotBtn3.GetComponent<Button>().enabled = false;
            if(slot3Save) slot3Img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if(slot3Save) slotBtn3.GetComponentInChildren<TMP_Text>().text = "Slot 3 - Used";
            break;
    }
}

    public void SetSaveSlot(int slot)
    {
        
        PlayerPrefs.SetInt("SaveSlot", slot);
        PlayerPrefs.SetInt("SaveSlot" + slot, 1);
        PlayerPrefs.Save();
        if(SaveManager.SlotExists(slot)) return;
        SaveManager.Save(slot, SaveManager.CreateNewSave());
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Title_Scene");
    }
}
