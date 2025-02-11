using UnityEngine;

public class MovePlayerOnce : MonoBehaviour
{
    private float _movementAmount = 3f;
    private bool _hasMoved = false;
    private PlayerMovementComponent _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovementComponent>();
    }

    private void Update()
    {
        if (!_hasMoved && Input.GetMouseButtonDown(0)) // Detects left mouse click
        {
            StartCoroutine(_playerMovement.AutoMoveLeft(_movementAmount, 0.3f));
            _hasMoved = true;
        }
    }
}
