using System.Collections;
using UnityEngine;
using TMPro;

public class FadeUiTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] tmpComponents; // Array of TMP components to fade
    [SerializeField] private float fadeSpeed = 1f; // Speed of fading

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop any running fade coroutine and start fading out
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeAllTMP(0f));
        }
    }

    private IEnumerator FadeAllTMP(float targetAlpha)
    {
        // Get the starting alpha from the first TMP component
        if (tmpComponents.Length == 0 || tmpComponents[0] == null) yield break;

        float startAlpha = tmpComponents[0].color.a;

        // Track the fading progress
        float elapsedTime = 0f;
        while (!Mathf.Approximately(startAlpha, targetAlpha))
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime * fadeSpeed);

            // Apply the new alpha to all TMP components
            foreach (var tmp in tmpComponents)
            {
                if (tmp == null) continue;

                Color currentColor = tmp.color;
                currentColor.a = newAlpha;
                tmp.color = currentColor;
            }

            yield return null;
        }

        // Ensure the final alpha is set
        foreach (var tmp in tmpComponents)
        {
            if (tmp == null) continue;

            Color finalColor = tmp.color;
            finalColor.a = targetAlpha;
            tmp.color = finalColor;
        }

        fadeCoroutine = null; // Clear the coroutine reference
    }
}

