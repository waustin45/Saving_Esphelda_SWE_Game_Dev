using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Charged Jump Settings")]
    public float chargedHoldThreshold = 5f;
    public float chargedJumpForce = 10f;
    public float maxHoldTime = 10f;

    [Header("Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("References")]
    public SPUM_Prefabs spumScript;
    public WallClimbBoxCastAuto wallClimbScriptReference; // drag the WallClimbBoxCastAuto component here

    [Header("Audio")]
    public AudioSource walkingSounds;
    public AudioSource LandSounds;

    private Rigidbody2D rb;
    private float horizontal;
    private bool isGrounded;
    private bool isMoving;
    private WallClimbBoxCastAuto wallClimb;
    private bool preparingJump;
    private float holdTimer;
    private bool groundedAtPress;
    private bool wasGrounded;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spumScript != null)
        {
            spumScript.OverrideControllerInit();
        }

        if (wallClimbScriptReference != null)
        {
            wallClimb = wallClimbScriptReference as WallClimbBoxCastAuto;
            if (wallClimb == null)
            {
                Debug.LogWarning("wallClimbScriptReference is not a WallClimbBoxCastAuto component.");
            }
        }
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        bool shouldPlayWalking = isGrounded && Mathf.Abs(horizontal) > 0.1f;
        if (shouldPlayWalking && !walkingSounds.isPlaying)
        {
            walkingSounds.Play();
        }
        else if (!shouldPlayWalking && walkingSounds.isPlaying)
        {
            walkingSounds.Stop();
        }

        // Begin preparing a jump when the player presses Jump while grounded
        if (!wasGrounded && isGrounded)
        {
            LandSounds.Play();
        }
        wasGrounded = isGrounded;

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                preparingJump = true;
                holdTimer = 0f;
                groundedAtPress = true;
            }
            else
            {
                preparingJump = false;
                groundedAtPress = false;
            }
        }

        // While holding the jump button, increment the timer (only if we started preparing)
        if (preparingJump && Input.GetButton("Jump"))
        {
            holdTimer += Time.deltaTime + .02f;
            //Debug.Log(holdTimer);
            if (holdTimer > maxHoldTime) holdTimer = maxHoldTime;
            
        }

        // On release: perform the jump based on how long it was held
        if (preparingJump && Input.GetButtonUp("Jump"))
        {
            bool isClimbing = wallClimb != null && wallClimb.IsClimbingPublic;
            Debug.Log(isClimbing);

            // Only perform normal/charged jump if not currently climbing
            if (!isClimbing && groundedAtPress)
            {
                if (holdTimer >= chargedHoldThreshold)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, chargedJumpForce);
                }
                else
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                }
            }

            preparingJump = false;
            holdTimer = 0f;
            groundedAtPress = false;
        }

        // If the player lands while preparing or not holding, clear the state
        if (isGrounded && !Input.GetButton("Jump"))
        {
            preparingJump = false;
            holdTimer = 0f;
            groundedAtPress = false;
        }

        HandleAnimations();
    }

    void HandleAnimations()
    {
        if (spumScript == null) return;

        bool movingNow = Mathf.Abs(horizontal) > 0.1f;

        if (movingNow != isMoving)
        {
            isMoving = movingNow;

            if (isMoving) spumScript.PlayAnimation(PlayerState.MOVE, 0);
            else spumScript.PlayAnimation(PlayerState.IDLE, 0);
        }

        if (horizontal > 0) spumScript.transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0) spumScript.transform.localScale = new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // If wall climb script exists, check climbing state
        bool isClimbing = wallClimb != null && wallClimb.IsClimbingPublic;

        // If climbing, don't override vertical velocity and optionally reduce horizontal movement
        if (isClimbing)
        {
            // Stop horizontal movement while climbing so player doesn't keep pushing into the wall
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            // Normal horizontal movement
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    
}
