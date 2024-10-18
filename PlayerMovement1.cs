using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement")]
    public float horizontal;
    private float maxSpeed = 10f;
    public float accelerationSpeed;
    public float speed = 5f;
    public float rollSpeed = 50f;
    public int rollingDamage = 100;
    public int extraJumps = 1; // Change to int for double jump count

    [Header("Checks")]
    public bool isFacingRight = true;
    public bool isJumping;
    public bool isRolling;
    public bool canRoll;
    public bool canJump;

    public GameObject RollHitBox;
    public bool rollOnCoolDown;

    public bool isIdle;
    public bool isMoving;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;
    private Vector2 dashingDir;
    public bool isDashing;
    private bool canDash = true;

    // Ground Checks
    [SerializeField] private Transform groundCheck;
    [SerializeField] public Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask groundLayer;
    // Wall Checks (for wall jumping/sliding)
    [SerializeField] private Transform wallCheck;
    [SerializeField] public Vector2 wallCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask wallLayer;

    [Header("WallMovement")]
    public bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    public bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.1f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Jumping")]
    public float jumpingPower = 16f;
    private float coyoteTime = 0.2f; // How long after leaving a platform can I jump
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f; // Buffer time for jump input
    private float jumpBufferCounter;

    private float jumpDelay = 0.2f; // Delay before allowing a double jump
    private bool canDoubleJump = true; // Flag to check if double jump is allowed

    [Header("Animations")]
    public Animator animator;
    private TrailRenderer trailRenderer;

    // Animation States
    private void Start()
    {
        RollHitBox.SetActive(false);
        rollOnCoolDown = false;
        canRoll = true;
        canJump = true;
        animator = gameObject.GetComponent<Animator>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    #region Movement / Update Functions
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Basically all the movement is in this

        // Animations
        if (horizontal == 0 && !isJumping && !isRolling && !isDashing && !isWallSliding) // If doing nothing at all (play Idle)
        {
            isIdle = true;
            speed = 5f;
        }
        else
        {
            isIdle = false;
        }

        if (horizontal != 0 && !isJumping && !isRolling && !isDashing && !isWallSliding) // If moving and not jumping or rolling
        {
            isMoving = true;

            if (IsGrounded())
            {
                // Play the running sound effect if it’s not already playing
                Sound s = Array.Find(FindObjectOfType<AudioManager>().sounds, sound => sound.name == "RunSFX");
                if (s != null && !s.source.isPlaying) // Check if the sound is already playing
                {
                    FindObjectOfType<AudioManager>().Play("RunSFX");
                }
            }

            if (speed < maxSpeed)
            {
                speed += Time.deltaTime * accelerationSpeed; // Speed up as you run
            }
        }
        else
        {
            isMoving = false;
            FindObjectOfType<AudioManager>().Stop("RunSFX"); // Stop running sound effect if not moving
        }

        Roll();
        Jump();
        Flip();
        Dash();
        WallSlide();
        WallJump();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
    #endregion

    #region JumpManager

    public void Jump()
    {
        // Check if grounded
        if (IsGrounded())
        {
            // Reset jump-related variables
            coyoteTimeCounter = coyoteTime;
            jumpBufferCounter = 0f; // Reset jump buffer when grounded
            extraJumps = 1; // Reset extra jumps when grounded
            canDoubleJump = true; // Allow double jump again
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time if not grounded
        }

        // Jump buffer handling
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime; // Reset the jump buffer counter when button is pressed
        }

        if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || canDoubleJump))
        {
            jumpBufferCounter = 0f; // Consume jump buffer

            // If it's the first jump, just jump
            if (IsGrounded())
            {
                PerformJump(jumpingPower); // Regular jump
            }
            // If it's the double jump, check if it can be executed
            else if (canDoubleJump)
            {
                StartCoroutine(JumpCooldown()); // Start cooldown for double jump
                PerformJump(jumpingPower * 0.8f); // Use reduced jump power for double jump
                FindObjectOfType<AudioManager>().Play("JumpSFX"); // Play jump sound here
                canDoubleJump = false; // Disable further double jumps until reset
            }
        }

        // For cutting the jump short when releasing the jump button
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f; // Reset coyote time when jump button is released
        }

        // Decrease jump buffer if it's active
        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime; // Decrease jump buffer
        }
    }

    // Separate method to handle the actual jump
    private void PerformJump(float power)
    {
        rb.velocity = new Vector2(rb.velocity.x, power);
        
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpDelay); // Wait for the specified delay
        isJumping = false; // Reset isJumping after the delay
    }

    #endregion

    #region RollManager
    public void Roll()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.LeftShift) && canRoll)
        {
            isRolling = true;
            canJump = false;
            speed = rollSpeed;
            RollHitBox.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && (isRolling))
        {
            isRolling = false;
            canJump = true;
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            StartCoroutine(RollCooldown());
            speed = 5f;
            RollHitBox.SetActive(false);
        }
    }

    // The timer between being able to Roll
    private IEnumerator RollCooldown()
    {
        rollOnCoolDown = true;
        canRoll = false;
        yield return new WaitForSeconds(3.0f);
        canRoll = true;
        rollOnCoolDown = false;
    }
    #endregion

    #region DashManager
    public void Dash()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            isDashing = true;
            canDash = false;

            rb.gravityScale = 0f;
            rb.velocity = new Vector2(transform.localScale.x * dashingVelocity, 0f);
            trailRenderer.emitting = true;
            dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rb.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }

        if (IsGrounded())
        {
            canDash = true;
        }
    }
    #endregion

    #region WallMovement
    private void WallSlide()
    {
        isWallSliding = AgainstWall() && !IsGrounded() && rb.velocity.y < 0;

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    #endregion

    #region ImportantAdditions

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180, 0f);
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }

    // GIZMOS 
    private void OnDrawGizmosSelected()
    {
        // Ground check visual
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
    #endregion

    #region Checks

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool AgainstWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    #endregion
}
