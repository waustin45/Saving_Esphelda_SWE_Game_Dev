using System.Collections;
using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource CollectSound;
    [Header("Inventory")]
    public PlayerInventory inventory;
    private bool IsCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !IsCollected)
        {
            Debug.Log("Collectible collected!");
            inventory.KeyCount++;
            IsCollected = true;
            StartCoroutine(PlayAndDestroy());
        }
    }

    IEnumerator PlayAndDestroy()
    {
        CollectSound.Play();
        yield return new WaitWhile(() => CollectSound.isPlaying);
        Destroy(gameObject);
    }
}

