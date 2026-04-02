using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int KeyCount { get; private set; } = 0;
    public int MaxKeys = 3;

    public int GemCount { get; private set; } = 0;

    public void CollectibleCollected(Collider other)
    {
        if (other.CompareTag("Key") && KeyCount < MaxKeys)
        {
            KeyCount++;
        }

        if (other.CompareTag("Gem"))
        {
            GemCount++;
        }
    }
}

