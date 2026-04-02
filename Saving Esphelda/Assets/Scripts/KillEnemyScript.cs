using UnityEngine;

public class KillEnemyScript : MonoBehaviour
{
[Header("Detection")]
    public Transform EnemyCheck;
    public float detectionRadius = 0.3f; // Slightly larger often feels better for stomp

    [Header("Settings")]
    public float bounceForce = 12f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() // Using Update for responsive input/checks
    {
        // 1. Get all colliders touching the feet sensor
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(EnemyCheck.position, detectionRadius);

        foreach (Collider2D obj in hitObjects)
        {
            // 2. Check if the object has the "Enemy" tag
            if (obj.CompareTag("Enemy"))
            {
                // 3. Only stomp if falling (Velocity.y is negative or near zero)
                if (rb.linearVelocity.y < 0.1f)
                {
                    StompEnemy(obj.gameObject);
                    break; // Exit loop so we don't stomp the same enemy twice in one frame
                }
            }
        }
    }

    void StompEnemy(GameObject enemy)
    {
        // Give the player a bounce
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);

        // Access the SPUM script on the enemy to play Death animation
        SPUM_Prefabs enemySpum = enemy.GetComponentInChildren<SPUM_Prefabs>();
        if (enemySpum != null)
        {
            enemySpum.PlayAnimation(PlayerState.DEATH, 0);
        }

        // Disable the enemy's logic so they stop moving/hurting you
        if (enemy.TryGetComponent(out Collider2D col)) col.enabled = false;
        
        // This stops the Patrol script we wrote earlier
        MonoBehaviour patrol = enemy.GetComponent<MonoBehaviour>(); 
        if (patrol != null) patrol.enabled = false;

        // Destroy the object after a short delay so the death animation plays
        Destroy(enemy, 1.5f);
        
        Debug.Log("Stomped " + enemy.name);
    }

    private void OnDrawGizmos()
    {
        if (EnemyCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(EnemyCheck.position, detectionRadius);
        }
    }
}
