using UnityEngine;

public class KeyObjectComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerKeyComponent key = other.GetComponent<PlayerKeyComponent>();
            var sound = gameObject.GetComponent<AudioSource>();
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            // Set the opacity of the SpriteRenderer to 0
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 0; // Set alpha to 0
                spriteRenderer.color = color;
            }

            if (sound != null)
            {
                sound.Play();
                key.ObtainKey();
                Destroy(gameObject, 0.3f); // Delay destruction to allow sound to play
            }
            else
            {
                key.ObtainKey();
                Destroy(gameObject); // Immediate destruction if no sound
            }
        }
    }
}
