using UnityEngine;

public class ObjectInputButtonComponent : MonoBehaviour
{
    [SerializeField] private KeyCode _keyCode;

    [Header("Overlap Settings")]
    [SerializeField] private LayerMask _overlapLayer;
    [SerializeField] private float _overlapRadius = 0.2f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform _groundCheck;  // Position to check if on the ground
    [SerializeField] private float _groundCheckRadius = 0.2f;  // Radius for ground check
    [SerializeField] private LayerMask _groundLayer;  // Ground layer to check against

    [Header("Sprite Settings")]
    [SerializeField] private Sprite _wKeySprite;
    [SerializeField] private Sprite _aKeySprite;
    [SerializeField] private Sprite _sKeySprite;
    [SerializeField] private Sprite _dKeySprite;
    [SerializeField] private Sprite _spaceKeySprite;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetUpSprite();
    }

    void Update()
    {
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, _overlapRadius, _overlapLayer);

        bool isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        if (overlap != null && isGrounded)
        {
            if (!GameInputManager.Instance.Is2ButtonsActive())
            {
                EnableKey();
                Destroy(gameObject);
            }
        }
    }

    // Optional: Visualize the overlap radius and ground check in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _overlapRadius);

        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }

    private void SetUpSprite()
    {
        switch (_keyCode)
        {
            case KeyCode.W:
                _spriteRenderer.sprite = _wKeySprite;
                break;
            case KeyCode.A:
                _spriteRenderer.sprite = _aKeySprite;
                break;
            case KeyCode.S:
                _spriteRenderer.sprite = _sKeySprite;
                break;
            case KeyCode.D:
                _spriteRenderer.sprite = _dKeySprite;
                break;
            case KeyCode.Space:
                _spriteRenderer.sprite = _spaceKeySprite;
                break;
        }
    }

    private void EnableKey()
    {
        if (GameInputManager.Instance != null)
        {
            GameInputManager.Instance.SetButtonState(_keyCode.ToString(), true);
        }
    }

    public void SetKeyCode(KeyCode keyCode)
    {
        _keyCode = keyCode;
        SetUpSprite();
    }
}
