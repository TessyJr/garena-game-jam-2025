using UnityEngine;

public class Blink : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Time in seconds between blinks

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            timer = 0f;
        }
    }
}
