using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        
        if (inventory != null)
        {            
            inventory.CollectibleCollected(other);
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit player");
            Destroy(gameObject);
        }

    }
}
