using UnityEngine;

public class KeyObjectComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerKeyComponent key = other.GetComponent<PlayerKeyComponent>();
            key.ObtainKey();
            Destroy(gameObject);
        }
    }
}