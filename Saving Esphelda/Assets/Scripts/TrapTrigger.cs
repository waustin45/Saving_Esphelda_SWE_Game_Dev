using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapTrigger : MonoBehaviour
{
    [Tooltip("If true, trap uses trigger events. If false, uses collision events.")]
    public bool useTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger) return;
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject other)
    {
        if (!other.CompareTag("Player")) return;

        // Type-safe call to PlayerDeath
        var death = other.GetComponent<PlayerDeath>();
        if (death != null)
        {
            death.KillPlayer();
            Debug.Log("TrapTrigger called PlayerDeath.KillPlayer");
            return;
        }

        // Fallback to SendMessage only if PlayerDeath is missing
        other.SendMessage("KillPlayer", SendMessageOptions.DontRequireReceiver);
        Debug.Log("TrapTrigger fallback SendMessage KillPlayer");
    }
}
