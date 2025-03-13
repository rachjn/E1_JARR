using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpHeight = 7f;

    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] int maxJumps = 2;

    [SerializeField] Vector2 teleportPosition = new Vector2(-20, 7);
    public Animator animator;
    [HideInInspector] public bool ledgeDetected;
    [Header("Ledge info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    [SerializeField] private float knockbackForce = 10f;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;
    private float ledgeGrabCooldown = 0.3f; // Cooldown after jumping before grabbing ledge
    private float ledgeGrabCooldownTimer = 0f;

    Vector2 dashDirection;
    bool isDashing = false;
    float dashCooldownTimer = 0f;
    float dashTimer = 0f;

    float direction = 0;
    float numJumps = 0;
    bool isFacingRight = true;


    public float minPitch = 0.1f; // Lower pitch limit
    public float maxPitch = 0.5f; // Upper pitch limit

     AudioManager audioManager;

    void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        var playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            if (gameObject.name.Contains("Player1"))
            {
                playerInput.SwitchCurrentActionMap("Player1");
                playerInput.SwitchCurrentControlScheme("Keyboard1", Keyboard.current);
            }
            else if (gameObject.name.Contains("Player2"))
            {
                playerInput.SwitchCurrentActionMap("Player2");
                playerInput.SwitchCurrentControlScheme("Keyboard2", Keyboard.current);
            }
        }
    }

    void Update()
    {
        dashCooldownTimer -= Time.deltaTime;
        ledgeGrabCooldownTimer -= Time.deltaTime;
        
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
        else
        {
            Move(direction);
        }

        CheckForLedge();
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge && ledgeGrabCooldownTimer <= 0f)
        {
            canGrabLedge = false;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            
            if (isFacingRight)
            {
                climbBegunPosition = ledgePosition + offset1;
            }
            else
            {
                climbBegunPosition = ledgePosition + new Vector2(-offset1.x, offset1.y);
            }

            climbOverPosition = ledgePosition + offset2;
            canClimb = true;
        }

        if (canClimb)
        {
            transform.position = climbBegunPosition;
            animator.SetBool("IsHanging", true);
        }
    }

    public void Knockback(Vector2 dir)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Reset velocity before applying knockback
            rb.gravityScale = 0; // Disable gravity temporarily
            isDashing = true; // Reuse dash logic for knockback
            dashTimer = 0.2f; // Set a short duration for knockback
            rb.linearVelocity = dir.normalized * knockbackForce; // Apply force in bullet's direction

            Invoke(nameof(EndKnockback), 0.1f);
        }
    }

    private void EndKnockback()
    {
        isDashing = false;
        rb.gravityScale = 1; // Re-enable gravity
    }

    void OnMove(InputValue value)
    {
        float v = value.Get<float>();
        if (v != 0 && (v > 0 != isFacingRight))
        {
            Flip();
        }
        direction = v;
    }

    void Move(float dir)
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
            animator.SetFloat("Speed", Mathf.Abs(dir * speed));
        }
    }

    void OnJump()
    {
        if (numJumps > 0 || canClimb)
        {
            canClimb = false;
            canGrabLedge = true;
            ledgeGrabCooldownTimer = ledgeGrabCooldown; // Reset cooldown timer after jumping
            animator.SetBool("IsHanging", false);
            // audioManager.SFXSource.pitch = Random.Range(minPitch, maxPitch);
            audioManager.PlaySFX(audioManager.jump);
            Jump();
            numJumps -= 1;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f)
                {
                    numJumps = maxJumps;
                }
            }
        }
    }

    void OnDash()
    {
        if (dashCooldownTimer <= 0f && !isDashing)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        audioManager.PlaySFX(audioManager.dash);
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        int dir = isFacingRight ? 1 : -1;
        dashDirection = new Vector2(dir, 0).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;
        rb.gravityScale = 0;
    }

    void EndDash()
    {
        isDashing = false;
        rb.gravityScale = 1;
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Dead"))
    //     {
    //         Debug.Log("Dead");
    //         transform.position = Vector2.zero;
    //     }
    // }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
