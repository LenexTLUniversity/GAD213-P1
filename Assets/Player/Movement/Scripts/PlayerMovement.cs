using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float horizontalJumpForce = 5f;
    public float gravityScale = 2.5f;

    public Rigidbody2D rb;

    private bool isJumping = false;
    private float jumpTimeCounter;
    private bool isGrounded = false;
    private bool facingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle turning with A/D keys
        if (Input.GetKeyDown(KeyCode.A))
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1); // Face Left
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            facingRight |= true;
            transform.localScale = new Vector3(1, 1, 1); // Face Right
        }


        // Check for jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Initial vertical jump force
            ApplyHorizontalJumpForce(); // Apply jump force based on direction faced
        }

        // Continue jumping while holding the spacebar (variable jump height)
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Continue vertical jump
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Stop jump if space is released early
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

    }


    private void ApplyHorizontalJumpForce()
    {
        if (facingRight)
        {
            rb.velocity = new Vector2(horizontalJumpForce, rb.velocity.y); // Jump right
        }
        else
        {
            rb.velocity = new Vector2(-horizontalJumpForce, rb.velocity.y); // Jump left
        }

    }
}
