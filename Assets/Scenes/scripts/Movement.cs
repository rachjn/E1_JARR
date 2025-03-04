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

    [SerializeField] Vector2 teleportPosition = new Vector2(-20, 7); // Target position for teleportation
    public Animator animator;


    // Animator anim;

    Vector2 dashDirection;
    bool isDashing = false;
    float dashCooldownTimer = 0f;
    float dashTimer = 0f;

    float direction = 0;
    float numJumps = 0;
    bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();
    }

    void Update()
    {
        dashCooldownTimer -= Time.deltaTime;
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
            // if ((isFacingRight && direction == -1) || (!isFacingRight && direction == 1)){
            //     Flip();
            // }
        }
    }

     void OnMove(InputValue value)
    {
        float v = value.Get<float>();
        if (v != 0 && (v > 0 != isFacingRight)) // Check if direction changed
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
            // anim.SetBool("isRunning", dir != 0); 
            animator.SetFloat("Speed", Mathf.Abs(dir * speed));
            Debug.Log("Speed: " + Mathf.Abs(dir * speed));
        }
    }

    void OnJump()
    {
        if (numJumps > 0)
        {
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
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        float dashX = direction != 0 ? direction : Mathf.Sign(rb.linearVelocity.x);
        dashDirection = new Vector2(dashX, 0).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;
        rb.gravityScale = 0;
    }

    void EndDash()
    {
        isDashing = false;
        rb.gravityScale = 1;
    }
    // void OnCollisionExit2D(Collision2D collision){
    //     if(collision.gameObject.CompareTag("Ground") ){
    //         for (int i = 0; i < collision.contactCount; i++)
    //         {
    //             if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) > 85f)
    //             {
    //                 numJumps -=1; 
    //             }
    //         }
    //      }
    // }
    void OnCollisionEnter2D(Collision2D collision){    
        if(collision.gameObject.CompareTag("Dead")){
            Debug.Log("dead");
            transform.position = new Vector2(0, 0);
         }
    }

//     void OnCollisionEnter2D(Collision2D collision)
// {
//     if (collision.gameObject.CompareTag("Enemy"))
//     {
//         GetComponent<HealthManager>().TakeDamage(20);
//     }
//     if (collision.gameObject.CompareTag("HealthPickup"))
//     {
//         GetComponent<HealthManager>().Heal(30);
//     }
// }

    // private void Flip(){
    //     isFacingRight = !isFacingRight; 
    //     Vector3 newLocalScale = transform.localScale;
    //     newLocalScale.x *= -1f;
    //     transform.localScale = newLocalScale; 
    // }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
