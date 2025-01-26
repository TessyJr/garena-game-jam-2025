using UnityEngine;
using System;

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

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _fallSound;

    [Header("Animator Settings")]
    [SerializeField] private Animator _animator;

    private Rigidbody2D rb;
    [SerializeField] private bool isGrounded;
    private bool facingRight = true;
    private bool movementEnabled = true;
    public event Action OnGrounded;

    private void Awake()
    {
        OnGrounded += PlayFallSound;
    }

    private void OnDestroy()
    {
        OnGrounded -= PlayFallSound;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground check logic
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Trigger the event if isGrounded changes to true
        if (!wasGrounded && isGrounded)
        {
            OnGrounded?.Invoke();
        }

        HandleMovement();
        HandleJumping();
        HandleClimbing();
    }

    private void PlayFallSound()
    {
        if (_fallSound != null)
        {
            _fallSound.Play();
        }
    }

    private void HandleMovement()
    {
        if (_menuCanvasManager != null)
        {
            if (!movementEnabled || _menuCanvasManager._isSpectating)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                _animator.SetTrigger("idle"); // Trigger "idle" animation
                return;
            }
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

        if (Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            _animator.SetTrigger("move"); 
        }
        else
        {
            _animator.SetTrigger("idle");
        }

        if (moveInput > 0 && facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && !facingRight)
        {
            Flip();
        }
    }

    private void HandleJumping()
    {
        if (_menuCanvasManager == null || !GameInputManager.Instance.buttonSpace || !Input.GetKeyDown(KeyCode.Space) || !isGrounded || !movementEnabled || _menuCanvasManager._isSpectating)
        {
            return;
        }
        _dust.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }


    private void HandleClimbing()
    {
        float moveInput = 0f;

        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ladder")))
        {
            if (GameInputManager.Instance.buttonW && Input.GetKey(KeyCode.W) && !_menuCanvasManager._isSpectating)
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
