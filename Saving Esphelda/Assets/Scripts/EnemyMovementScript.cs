using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
[Header("Movement Settings")]
    public float moveSpeed = 3f;
    public bool movingRight = true;

    [Header("Detection")]
    public Transform wallCheck;
    public Transform ledgeCheck;
    public Transform groundCheck;
    public float detectionRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("References")]
    public SPUM_Prefabs spumScript;

    [Header("Audio")]
    public AudioSource walkingSounds;


    private Rigidbody2D rb;
    private float flipCooldown = 0.75f; // Prevents rapid jittering
    private float lastFlipTime;
    private bool isGrounded;

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
        isGrounded = groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, detectionRadius, groundLayer);

        bool shouldPlayWalking = isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        if (shouldPlayWalking && !walkingSounds.isPlaying)
            walkingSounds.Play();
        else if (!shouldPlayWalking && walkingSounds.isPlaying)
            walkingSounds.Stop();

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
