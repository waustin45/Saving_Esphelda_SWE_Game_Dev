using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
[Header("Movement Settings")]
    public float moveSpeed = 3f;
    public bool movingRight = true;

    [Header("Detection")]
    public Transform wallCheck;
    public Transform ledgeCheck;
    public float detectionRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("References")]
    public SPUM_Prefabs spumScript;

    private Rigidbody2D rb;
    private float flipCooldown = 0.75f; // Prevents rapid jittering
    private float lastFlipTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spumScript != null)
        {
            spumScript.OverrideControllerInit();
            spumScript.PlayAnimation(PlayerState.MOVE, 0);
            UpdateSpriteDirection();
        }
    }

    void FixedUpdate()
    {
        // 1. Detection
        bool hitWall = Physics2D.OverlapCircle(wallCheck.position, detectionRadius, groundLayer);
        bool hitLedge = !Physics2D.OverlapCircle(ledgeCheck.position, detectionRadius, groundLayer);

        // 2. Flip Logic (with safety timer)
        if ((hitWall || hitLedge) && Time.time > lastFlipTime + flipCooldown)
        {
            movingRight = !movingRight;
            lastFlipTime = Time.time;
            UpdateSpriteDirection();
        }

        // 3. Movement
        float currentSpeed = movingRight ? moveSpeed : -moveSpeed;
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    void UpdateSpriteDirection()
    {
        if (spumScript == null) return;
        // Right = -1, Left = 1 (matching your player script)
        spumScript.transform.localScale = movingRight ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }
}
