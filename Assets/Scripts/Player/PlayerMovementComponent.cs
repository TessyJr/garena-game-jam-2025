using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementComponent : MonoBehaviour
{
    [Header("Menu Canvas Settings")]
    [SerializeField] private MenuCanvasManager _menuCanvasManager;

    [Header("Movement Settings")]
    [SerializeField] private Direction _direction = Direction.Left;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float climbSpeed = 4f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundLayer;

    [Header("Pipe Check Settings")]
    [SerializeField] private Transform pipeCheck;
    [SerializeField] private float pipeCheckRadius = 0.1f;
    [SerializeField] private LayerMask pipeLayer;
    [SerializeField] private PipeComponent currentPipe;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem _dust;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _fallSound;
    [SerializeField] private AudioSource _stairSound;
    [SerializeField] private AudioSource _teleportSound;

    private Rigidbody2D rb;
    private PlayerStateComponent _playerState;

    private bool movementEnabled = true;

    private bool autoMoving = false;

    public bool isGrounded;
    public bool _isJumping;
    public bool _isClimbing;
    public bool _isOnLadder = false;

    private float _horizontalMoveInput = 0f;
    private float _verticalMoveInput = 0f;

    public event Action OnGrounded;

    private void Awake()
    {
        _playerState = GetComponent<PlayerStateComponent>();
        rb = GetComponent<Rigidbody2D>();
        OnGrounded += PlayFallSound;
    }

    private void OnDestroy()
    {
        OnGrounded -= PlayFallSound;
    }

    private void Update()
    {
        CheckGroundedStatus();

        HandleAnimation();
        HandleMovement();
        HandleJumping();
        HandleClimbing();
        HandleTeleport();
    }


    private void CheckGroundedStatus()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            if (_isJumping)
            {
                _isJumping = false;
            }

            if (_isClimbing)
            {
                _isClimbing = false;
                _stairSound.Stop();
            }

            OnGrounded?.Invoke();
        }
    }

    private void HandleAnimation()
    {
        if (_horizontalMoveInput != 0)
        {
            if (_isClimbing)
            {
                _playerState.SetState(PlayerState.Climbing);
            }
            else if (_isJumping)
            {
                _playerState.SetState(PlayerState.Jumping);
            }
            else
            {
                _playerState.SetState(PlayerState.Walking);
            }
        }
        else
        {
            if (_isClimbing)
            {
                _playerState.SetState(PlayerState.Climbing);
            }
            else if (_isJumping)
            {
                _playerState.SetState(PlayerState.Jumping);
            }
            else
            {
                _playerState.SetState(PlayerState.Idle);
            }
        }
    }

    private void HandleMovement()
    {
        if (autoMoving || !movementEnabled || (_menuCanvasManager != null && _menuCanvasManager._isSpectating))
        {
            return;
        }

        if (Input.GetKey(KeyCode.A) && GameInputManager.Instance.buttonA)
        {
            _horizontalMoveInput = -1f;
            if (_direction == Direction.Right) Flip();
            _direction = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.D) && GameInputManager.Instance.buttonD)
        {
            _horizontalMoveInput = 1f;
            if (_direction == Direction.Left) Flip();
            _direction = Direction.Right;
        }
        else
        {
            _horizontalMoveInput = 0f;
        }

        rb.velocity = new Vector2(_horizontalMoveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleJumping()
    {
        if (!movementEnabled || (_menuCanvasManager != null && _menuCanvasManager._isSpectating) || _isClimbing || !isGrounded)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameInputManager.Instance.buttonSPACE)
        {
            _isJumping = true;
            _dust.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleClimbing()
    {
        if (!movementEnabled || (_menuCanvasManager != null && _menuCanvasManager._isSpectating))
        {
            return;
        }

        _isOnLadder = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, LayerMask.GetMask("Ladder"));

        if (_isOnLadder)
        {
            if (Input.GetKey(KeyCode.W) && GameInputManager.Instance.buttonW)
            {
                _isClimbing = true;
                _verticalMoveInput = 1f;
            }
            else
            {
                if (_isClimbing)
                {
                    _verticalMoveInput = -0.5f;
                }
            }

            if (_isClimbing)
            {
                rb.velocity = new Vector2(rb.velocity.x, _verticalMoveInput * climbSpeed);

                if (Mathf.Abs(rb.velocity.y) > 0.1f && !_stairSound.isPlaying)
                {
                    _stairSound.Play();
                }
                else if (Mathf.Abs(rb.velocity.y) < 0.1f && _stairSound.isPlaying)
                {
                    _stairSound.Stop();
                }
            }
        }
        else
        {
            _isClimbing = false;
            _verticalMoveInput = 0f;

            if (_stairSound.isPlaying)
            {
                _stairSound.Stop();
            }

            return;
        }
    }

    private void HandleTeleport()
    {
        if (!movementEnabled || (_menuCanvasManager != null && _menuCanvasManager._isSpectating) || _isClimbing || !isGrounded)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, groundCheckSize, pipeLayer);

        if (colliders.Length > 0)
        {
            currentPipe = colliders[0].gameObject.GetComponentInParent<PipeComponent>();
        }
        else
        {
            currentPipe = null;
        }

        if (Input.GetKeyDown(KeyCode.S) && GameInputManager.Instance.buttonS)
        {
            if (currentPipe != null && currentPipe.LinkedPipe != null)
            {
                _teleportSound.Play();
                transform.position = currentPipe.LinkedPipe.GetChild(0).position;
            }
        }

    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (isGrounded)
        {
            _dust.Play();
        }
    }

    private void PlayFallSound()
    {
        _fallSound?.Play();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }

        if (pipeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(pipeCheck.position, pipeCheckRadius);
        }
    }

    public Direction GetDirection() => _direction;

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
        if (!enabled) rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public IEnumerator AutoMoveLeft(float autoMoveDistance, float duration)
    {
        autoMoving = true;
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = rb.position + Vector2.left * autoMoveDistance;
        float elapsed = 0f;
        _horizontalMoveInput = -1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsed / duration));
            yield return null;
        }
        rb.MovePosition(targetPosition);
        _horizontalMoveInput = 0f;
        autoMoving = false;
    }
}
