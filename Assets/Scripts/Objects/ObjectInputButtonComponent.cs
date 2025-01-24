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

    private void Update()
    {
        // Check if the object is overlapping with the designated layer
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, _overlapRadius, _overlapLayer);

        // Check if the object is on the ground
        bool isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        // Only enable the key if the object is on the ground
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

    private void EnableKey()
    {
        if (GameInputManager.Instance != null)
        {
            GameInputManager.Instance.SetButtonState(_keyCode.ToString(), true);
        }
    }

    public void SetKeyCode(KeyCode keyCode) => _keyCode = keyCode;
}
