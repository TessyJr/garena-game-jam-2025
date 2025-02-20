using UnityEngine;

public class ObjectInputButtonComponent : MonoBehaviour
{
    [SerializeField] private KeyCode _keyCode;

    [Header("Chekcer Settings")]
    [SerializeField] private BoxCollider2D _groundChecker;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite _wKeySprite;
    [SerializeField] private Sprite _aKeySprite;
    [SerializeField] private Sprite _sKeySprite;
    [SerializeField] private Sprite _dKeySprite;
    [SerializeField] private Sprite _spaceKeySprite;

    [Header("State Settings")]
    public bool _isGrounded;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        SetUpSprite();
    }

    void Update()
    {
        Collider2D _isOverlapWithPlayer = Physics2D.OverlapBox(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, LayerMask.GetMask("Player"));

        _isGrounded = Physics2D.OverlapBox(_groundChecker.bounds.center, _groundChecker.bounds.size, 0, LayerMask.GetMask("Ground", "Pipe"));

        if (_isOverlapWithPlayer != null && _isGrounded)
        {
            if (!GameInputManager.Instance.Is2ButtonsActive())
            {
                EnableKey();
                Destroy(gameObject);
            }
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
