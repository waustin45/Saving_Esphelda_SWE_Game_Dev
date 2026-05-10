using System.Collections;
using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource CollectSound;
    [Header("Inventory")]
    public PlayerInventory inventory;
    private bool IsCollected = false;
    public string keyID;

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Player") && !IsCollected)
    {
        if (PlayerInventory.HasCollectedKey(keyID))
        {
            Destroy(gameObject);
            return;
        }

        inventory.AddKey();

        PlayerInventory.MarkKeyCollected(keyID);

        IsCollected = true;
        SetCollectedVisualState();
        PlayCollectSound();

        Destroy(gameObject, 0.1f);
    }
}

    private void SetCollectedVisualState()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        foreach (var collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (var collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }

    private void PlayCollectSound()
    {
        if (CollectSound == null || CollectSound.clip == null)
            return;

        AudioSource.PlayClipAtPoint(CollectSound.clip, transform.position);
    }
}


