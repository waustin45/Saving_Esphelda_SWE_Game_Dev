using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int KeyCount;
    public int MaxKeys = 3;
    public int GemCount;

    public GameObject[] keySlots;
    public string emptyKey = "Empty";
    public string filledKey = "Filled";

    void Start()
    {
        UpdateKeyHUD();
    }

    public void AddKey()
    {
        KeyCount = Mathf.Clamp(KeyCount + 1, 0, MaxKeys);
        UpdateKeyHUD();
    }

    private void UpdateKeyHUD()
    {
        for (int i = 0; i < keySlots.Length; i++)
        {
            if (keySlots[i] == null)
                continue;

            var filledChild = keySlots[i].transform.Find(filledKey);
            var emptyChild = keySlots[i].transform.Find(emptyKey);

            if (filledChild != null && emptyChild != null)
            {
                filledChild.gameObject.SetActive(i < KeyCount);
                emptyChild.gameObject.SetActive(i >= KeyCount);
            }
        }
    }
}

