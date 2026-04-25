using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Tooltip("Movement or input script to disable on death")]
    public MonoBehaviour playerController;

    [Tooltip("Colliders to disable so player doesn't block level")]
    public Collider2D[] collidersToDisable;

    [Tooltip("Downward velocity applied when killed")]
    public float fallVelocity = -12f;

    // Public method traps and enemies will call
    public void KillPlayer()
    {
        // Play death animation via SPUM_Prefabs if present
        var spum = GetComponentInChildren<SPUM_Prefabs>();
        if (spum != null) spum.PlayAnimation(PlayerState.DEATH, 0);

        // Disable player control
        if (playerController != null) playerController.enabled = false;

        // Disable colliders so player doesn't block the level
        foreach (var c in collidersToDisable)
            if (c != null) c.enabled = false;

        // Make player fall visibly
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = new Vector2(rb.linearVelocity.x, fallVelocity);

        // Optional: notify GameManager or start respawn coroutine here
        Debug.Log("PlayerDeath.KillPlayer called");
    }
}
