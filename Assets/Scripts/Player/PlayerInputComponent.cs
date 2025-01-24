using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] GameObject _objectToSpawn;
    [SerializeField] float _spawnForce = 4f;

    private PlayerMovementComponent _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovementComponent>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _playerMovement.SetMovementEnabled(false);
            // Disable A
            if (Input.GetKeyDown(KeyCode.A) && GameInputManager.Instance.IsButtonActive("A"))
            {

                GameInputManager.Instance.SetButtonState("A", false);
                Debug.Log("Button A disabled");
                DropObject(KeyCode.A);
            }

            // Disable D
            if (Input.GetKeyDown(KeyCode.D) && GameInputManager.Instance.IsButtonActive("D"))
            {

                GameInputManager.Instance.SetButtonState("D", false);
                Debug.Log("Button D disabled");
                DropObject(KeyCode.D);
            }

            // Disable SPACE
            if (Input.GetKeyDown(KeyCode.Space) && GameInputManager.Instance.IsButtonActive("SPACE"))
            {
                GameInputManager.Instance.SetButtonState("SPACE", false);
                Debug.Log("Button SPACE disabled");
                DropObject(KeyCode.Space);
            }
        }
        else
        {
            _playerMovement.SetMovementEnabled(true);
        }
    }

    private void DropObject(KeyCode keyCode)
    {
        if (_objectToSpawn != null)
        {
            // Spawn the object at the player's position
            GameObject spawnedObject = Instantiate(_objectToSpawn, transform.position, Quaternion.identity);
            spawnedObject.GetComponent<ObjectInputButtonComponent>().SetKeyCode(keyCode);

            // Add force based on facing direction
            Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 forceDirection = _playerMovement.GetFacingRight() ? Vector2.right : Vector2.left;
                rb.AddForce(forceDirection * _spawnForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("No object assigned to spawn!");
        }
    }
}
