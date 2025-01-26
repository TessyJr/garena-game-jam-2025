using UnityEngine;

public class KeyObjectComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerKeyComponent key = other.GetComponent<PlayerKeyComponent>();
            var sound = gameObject.GetComponent<AudioSource>();

            if (sound != null)
            {
                sound.Play();
                key.ObtainKey();
                Destroy(gameObject, 0.3f);
            }
            else
            {
                key.ObtainKey();
                Destroy(gameObject);
            }
        }
    }
}