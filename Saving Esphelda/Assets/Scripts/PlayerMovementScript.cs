using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    [Header("References")]
    public SPUM_Prefabs spumScript; // Drag the SPUM child object here

    private Rigidbody2D rb;
    private float horizontal;
    private bool isGrounded;
    private bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Safety check to initialize the SPUM controller
        if (spumScript != null)
        {
            spumScript.OverrideControllerInit();
        }
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump Logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // You could trigger a "Jump" or "Other" animation here if available
        }

        HandleAnimations();
    }

    void HandleAnimations()
    {
        if (spumScript == null) return;

        // Check if we are moving horizontally
        bool movingNow = Mathf.Abs(horizontal) > 0.1f;

        // Only update the SPUM state if our movement status changed
        if (movingNow != isMoving)
        {
            isMoving = movingNow;
            
            if (isMoving)
            {
                // Move state usually uses index 0 as default
                spumScript.PlayAnimation(PlayerState.MOVE, 0);
            }
            else
            {
                spumScript.PlayAnimation(PlayerState.IDLE, 0);
            }
        }

        // Flip the sprite based on direction
        if (horizontal > 0) spumScript.transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0) spumScript.transform.localScale = new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}