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
            if (inventory == null)
            {
                Debug.LogWarning("KeyCollectible: inventory reference is not set.");
                return;
            }

            Debug.Log("Collectible collected!");
            inventory.AddKey();
            IsCollected = true;

            //hide the key immediately so it appears collected faster.
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


