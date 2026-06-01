using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 16f;
    private bool isFacingRight = true;
    private bool jumpBuffer = false;
    private int airTime = 0;
    private Vector2 groundCheckSize = new Vector2(0.95f, 1f);

    public HealthSystem healthSystemRef;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform jumpBufferCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (!IsGrounded())
        {
            airTime += 1;
        } else {
            airTime = 0;
        }

        if (Input.GetButtonDown("Jump") && (IsGrounded() || airTime < 50 && rb.linearVelocity.y < 0f))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Jump") && CanJumpBuffer() && rb.linearVelocity.y < 0f)
        {
            jumpBuffer = true;
        }

        if (jumpBuffer == true && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpBuffer = false;
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            transform.position = new Vector2(0f, 0f);
        }

        if (DamageCheck())
        {
            healthSystemRef.DealDamage();
        }

        Flip();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (DamageCheck())
        {
            Vector3 contactPoint = collision.GetContact(0).point;
            Vector2 kbDir = (transform.position - contactPoint).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(kbDir * 10, ForceMode2D.Impulse);
            
            Debug.Log("Player knocked back in direction: " + kbDir);
        }
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < 50f)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        } else {
            rb.linearVelocity = new Vector2(horizontal * speed, 50f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
    }

    private bool CanJumpBuffer()
    {
        return Physics2D.OverlapBox(jumpBufferCheck.position, groundCheckSize, 0f, groundLayer);
    }

    private bool DamageCheck()
    {
        return Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0f, obstacleLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
