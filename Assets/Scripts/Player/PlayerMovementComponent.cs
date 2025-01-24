using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementComponent : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement based on active buttons
        HandleMovement();

        // Handle jumping based on active buttons
        HandleJumping();
    }

    private void HandleMovement()
    {
        float moveInput = 0f;

        // Check if buttonA (left movement) is active
        if (GameInputManager.Instance.buttonA && Input.GetKey(KeyCode.A))
        {
            moveInput = -1f; // Move left
        }

        // Check if buttonD (right movement) is active
        if (GameInputManager.Instance.buttonD && Input.GetKey(KeyCode.D))
        {
            moveInput = 1f; // Move right
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip the character sprite
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void HandleJumping()
    {
        // Check if buttonSpace (jump) is active
        if (GameInputManager.Instance.buttonSpace && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public bool GetFacingRight() => facingRight;
}
