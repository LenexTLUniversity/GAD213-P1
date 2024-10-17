using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float minJumpForce = 5f;          // Minimum jump force for a tap
    public float maxJumpForce = 12f;         // Maximum jump force for holding space
    public float maxHoldTime = 1f;           // Maximum time space can be held for full jump height
    public float horizontalJumpForce = 5f;   // Horizontal force applied during jump
    public float gravityScale = 2.5f;        // Gravity scale for faster falling
    public float smallBounceForce = 1f;      // Small bounce force for landing
    public float wallBounceMultiplier = 0.5f; // Multiplier for bounce back from walls
    public bool isHoldingJump = false;      // Track if the player is holding the space bar
    public float holdTime;                  // How long the space bar is held

    public Rigidbody2D rb;

    private bool isGrounded = false;
    private bool facingRight = true;         // Keep track of the direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;      // Set custom gravity for the player
    }

    void Update()
    {
        // Handle turning with A/D keys
        if (Input.GetKeyDown(KeyCode.A))
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }

        // Start holding jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isHoldingJump = true;
            holdTime = 0f; // Reset hold time
        }

        // Increment hold time while holding space
        if (Input.GetKey(KeyCode.Space) && isHoldingJump)
        {
            holdTime += Time.deltaTime; // Accumulate hold time
            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime); // Cap at maxHoldTime
        }

        // Jump when space is released
        if (Input.GetKeyUp(KeyCode.Space) && isHoldingJump)
        {
            isHoldingJump = false;
            Jump();
        }
    }

    private void Jump()
    {
        // Calculate jump force based on how long space was held
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / maxHoldTime);

        // Apply the jump force
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Apply horizontal force depending on the direction
        if (facingRight)
        {
            rb.velocity = new Vector2(horizontalJumpForce, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-horizontalJumpForce, rb.velocity.y);
        }
    }

    // Small bounce when landing
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Apply a small upward force for a subtle bounce
            rb.velocity = new Vector2(rb.velocity.x, smallBounceForce);
        }

        // Bounce back if hitting a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Calculate bounce force based on jump force
            float bounceBackForce = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / maxHoldTime) * wallBounceMultiplier;

            // Apply bounce back in the opposite direction of facing
            if (facingRight)
            {
                rb.velocity = new Vector2(-bounceBackForce, rb.velocity.y);  // Bounce left
            }
            else
            {
                rb.velocity = new Vector2(bounceBackForce, rb.velocity.y);  // Bounce right
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
