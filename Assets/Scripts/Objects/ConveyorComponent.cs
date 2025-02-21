using UnityEngine;

public class ConveyorComponent : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float speed = 2f;
    [SerializeField] private ForceMode2D forceMode = ForceMode2D.Force;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            if (other.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 conveyorForce = direction.normalized * speed;
                rb.AddForce(conveyorForce, forceMode);
            }
        }
    }
}


