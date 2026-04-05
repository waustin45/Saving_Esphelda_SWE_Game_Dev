using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WallClimbBoxCastAuto : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask wallLayer;                 // set to your Wall layer in inspector
    public Collider2D playerCollider;          // optional: will auto-find if left empty
    [Tooltip("How far from the player's side to check for a wall")]
    public float checkDistance = 0.08f;
    [Tooltip("Shrink factor for the boxcast size relative to the player's collider bounds")]
    [Range(0.1f, 1f)]
    public float boxSizeShrink = 0.9f;

    [Header("Climb Settings")]
    public float climbSpeed = 3.5f;
    public float climbSmoothing = 10f;
    public bool requireUpToClimb = true;

    [Header("Debug")]
    public bool debugDraw = true;

    private Rigidbody2D rb;
    private float originalGravity;
    private bool touchingWallLeft;
    private bool touchingWallRight;
    private float targetVerticalVelocity = 0f;

    // Public flag other scripts can read
    [HideInInspector] public bool IsClimbingPublic = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;

        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
            if (playerCollider == null)
            {
                Debug.LogError("WallClimbBoxCastAuto requires a Collider2D on the player or assign one in inspector.");
            }
        }
    }

    void Update()
    {
        DetectWalls();

        float verticalInput = Input.GetAxisRaw("Vertical"); // Up is positive
        bool wantsClimb = !requireUpToClimb || verticalInput > 0.1f;

        bool touchingWall = touchingWallLeft || touchingWallRight;

        // Enter climbing mode only when touching a wall, pressing Up (if required), and not grounded
        if (touchingWall && wantsClimb && !IsGrounded())
        {
            targetVerticalVelocity = Mathf.Clamp(verticalInput, -1f, 1f) * climbSpeed;
            rb.gravityScale = 0f;
            IsClimbingPublic = true;
        }
        else
        {
            targetVerticalVelocity = rb.linearVelocity.y;
            rb.gravityScale = originalGravity;
            IsClimbingPublic = false;
        }

        // No wall jump: climbing only controls vertical movement while touching wall
    }

    void FixedUpdate()
    {
        if (rb.gravityScale == 0f && (touchingWallLeft || touchingWallRight))
        {
            float newY = Mathf.Lerp(rb.linearVelocity.y, targetVerticalVelocity, climbSmoothing * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, newY);
        }
    }

    void DetectWalls()
    {
        touchingWallLeft = false;
        touchingWallRight = false;

        if (playerCollider == null) return;

        Bounds b = playerCollider.bounds;
        Vector2 size = new Vector2(b.size.x * boxSizeShrink, b.size.y * boxSizeShrink);

        // Left boxcast
        Vector2 leftOrigin = new Vector2(b.center.x - (b.extents.x + checkDistance * 0.5f), b.center.y);
        RaycastHit2D leftHit = Physics2D.BoxCast(leftOrigin, size, 0f, Vector2.left, checkDistance, wallLayer);
        touchingWallLeft = leftHit.collider != null;

        // Right boxcast
        Vector2 rightOrigin = new Vector2(b.center.x + (b.extents.x + checkDistance * 0.5f), b.center.y);
        RaycastHit2D rightHit = Physics2D.BoxCast(rightOrigin, size, 0f, Vector2.right, checkDistance, wallLayer);
        touchingWallRight = rightHit.collider != null;

        if (debugDraw)
        {
            DebugDrawBox(leftOrigin, size, Color.cyan);
            DebugDrawBox(rightOrigin, size, Color.cyan);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, ~0);
        return hit.collider != null && hit.collider.gameObject != gameObject;
    }

    void DebugDrawBox(Vector2 center, Vector2 size, Color color)
    {
        if (!debugDraw) return;
        Vector3 c = center;
        Vector3 s = new Vector3(size.x, size.y, 0f);
        Vector3 bl = c + new Vector3(-s.x, -s.y, 0f) * 0.5f;
        Vector3 br = c + new Vector3(s.x, -s.y, 0f) * 0.5f;
        Vector3 tl = c + new Vector3(-s.x, s.y, 0f) * 0.5f;
        Vector3 tr = c + new Vector3(s.x, s.y, 0f) * 0.5f;
        Debug.DrawLine(bl, br, color);
        Debug.DrawLine(br, tr, color);
        Debug.DrawLine(tr, tl, color);
        Debug.DrawLine(tl, bl, color);
    }

    void OnDrawGizmosSelected()
    {
        if (playerCollider == null) return;
        Gizmos.color = Color.cyan;
        Bounds b = playerCollider.bounds;
        Vector2 size = new Vector2(b.size.x * boxSizeShrink, b.size.y * boxSizeShrink);
        Vector2 leftOrigin = new Vector2(b.center.x - (b.extents.x + checkDistance * 0.5f), b.center.y);
        Vector2 rightOrigin = new Vector2(b.center.x + (b.extents.x + checkDistance * 0.5f), b.center.y);
        Gizmos.DrawWireCube(leftOrigin, size);
        Gizmos.DrawWireCube(rightOrigin, size);
    }
}
