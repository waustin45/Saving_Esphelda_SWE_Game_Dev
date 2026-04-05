using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public PlayerInventory inventory;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collectible collected!");
            inventory.KeyCount++;
            Destroy(gameObject);
        }
    }
}

