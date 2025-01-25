using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementComponent : MonoBehaviour
{
    [Header("Menu Canvas Settings")]
    [SerializeField] MenuCanvasManager _menuCanvasManager;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float climbSpeed = 4f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem _dust;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private bool movementEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!_menuCanvasManager._isSpectating)
        {
            HandleMovement();

            HandleJumping();

            HandleClimbing();
        }
    }

    private void HandleMovement()
    {
        if (!movementEnabled)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float moveInput = 0f;

        if (GameInputManager.Instance.buttonA && Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }

        if (GameInputManager.Instance.buttonD && Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

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
        if (GameInputManager.Instance.buttonSpace && Input.GetKeyDown(KeyCode.Space) && isGrounded && movementEnabled)
        {
            _dust.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleClimbing()
    {
        float moveInput = 0f;

        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ladder")))
        {
            Debug.Log($"Is On Ladder");

            if (GameInputManager.Instance.buttonW && Input.GetKey(KeyCode.W))
            {
                moveInput = 1f;
            }

            rb.velocity = new Vector2(rb.velocity.x, moveInput * climbSpeed);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (isGrounded)
        {
            _dust.Play();
        }
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

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
        if (!enabled)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

}
