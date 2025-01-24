using UnityEngine;

public class ConveyorComponent : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float speed = 2f;
    [SerializeField] private ForceMode2D forceMode = ForceMode2D.Force;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ButtonObject"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 conveyorForce = direction.normalized * speed;
                rb.AddForce(conveyorForce, forceMode);
            }
        }
    }
}


