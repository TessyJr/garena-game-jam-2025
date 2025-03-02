using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    [Header("Menu Canvas Settings")]
    [SerializeField] MenuCanvasManager _menuCanvasManager;

    [Header("Spawn Settings")]
    [SerializeField] GameObject _objectToSpawn;
    [SerializeField] float _spawnForce = 4f;

    [Header("Sound Settings")]
    [SerializeField] AudioSource _ejectSound;

    private PlayerMovementComponent _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovementComponent>();
    }

    void Update()
    {
        if (_menuCanvasManager == null || !_menuCanvasManager._isSpectating)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.A) && GameInputManager.Instance.IsButtonActive("A"))
                {
                    GameInputManager.Instance.SetButtonState("A", false);
                    DropObject(KeyCode.A);
                }
                else if (Input.GetKeyDown(KeyCode.D) && GameInputManager.Instance.IsButtonActive("D"))
                {
                    GameInputManager.Instance.SetButtonState("D", false);
                    DropObject(KeyCode.D);
                }
                else if (Input.GetKeyDown(KeyCode.W) && GameInputManager.Instance.IsButtonActive("W"))
                {
                    GameInputManager.Instance.SetButtonState("W", false);
                    DropObject(KeyCode.W);
                }
                else if (Input.GetKeyDown(KeyCode.S) && GameInputManager.Instance.IsButtonActive("S"))
                {
                    GameInputManager.Instance.SetButtonState("S", false);
                    DropObject(KeyCode.S);
                }
                else if (Input.GetKeyDown(KeyCode.Space) && GameInputManager.Instance.IsButtonActive("SPACE"))
                {
                    GameInputManager.Instance.SetButtonState("SPACE", false);
                    DropObject(KeyCode.Space);
                }
                else
                {
                    _playerMovement.HandleMovement();
                    _playerMovement.HandleJumping();
                    _playerMovement.HandleClimbing();
                    _playerMovement.HandleTeleport();
                }
            }
            else
            {
                _playerMovement.HandleMovement();
                _playerMovement.HandleJumping();
                _playerMovement.HandleClimbing();
                _playerMovement.HandleTeleport();
            }
        }
    }

    private void DropObject(KeyCode keyCode)
    {
        if (_objectToSpawn != null)
        {
            // Spawn the object at the player's position
            _ejectSound.Play();
            GameObject spawnedObject = Instantiate(_objectToSpawn, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
            spawnedObject.GetComponent<ObjectInputButtonComponent>().SetKeyCode(keyCode);

            // Add force based on facing direction
            Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                Vector2 forceDirection;
                if (_playerMovement.GetDirection() == Direction.Left)
                {
                    forceDirection = Vector2.left;
                }
                else
                {
                    forceDirection = Vector2.right;
                }

                rb.AddForce(forceDirection * _spawnForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("No object assigned to spawn!");
        }
    }
}
