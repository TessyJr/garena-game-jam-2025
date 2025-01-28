using UnityEngine;

public class Fade : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Time in seconds for a full blink cycle
    public float minOpacity = 0.2f;   // Minimum opacity
    public float maxOpacity = 1.0f;   // Maximum opacity

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool fadingOut;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetOpacity(maxOpacity); // Start fully visible
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            timer = 0f;
            fadingOut = !fadingOut; // Toggle between fading out and in
        }

        // Calculate opacity based on time and direction
        float t = timer / blinkInterval;
        float targetOpacity = fadingOut
            ? Mathf.Lerp(maxOpacity, minOpacity, t)
            : Mathf.Lerp(minOpacity, maxOpacity, t);

        SetOpacity(targetOpacity);
    }

    private void SetOpacity(float opacity)
    {
        Color color = spriteRenderer.color;
        color.a = opacity; // Modify alpha channel
        spriteRenderer.color = color;
    }
}
