using UnityEngine;
using System.Collections;

public class OpenBarrierTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _barrier;
    [SerializeField] private float _openDuration;

    [Header("Button Sprites")]
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    [SerializeField] private AudioSource _barrierAudioSource;

    [Header("Barrier Sprites")]
    [SerializeField] private Sprite _barrier1; // Closed
    [SerializeField] private Sprite _barrier2;
    [SerializeField] private Sprite _barrier3;
    [SerializeField] private Sprite _barrier4;
    [SerializeField] private Sprite _barrier5; // Opened

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _barrierSpriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _barrierSpriteRenderer = _barrier.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_barrier.activeSelf == true)
            {
                StartCoroutine(OpenBarrier());
            }
        }
    }

    private IEnumerator OpenBarrier()
    {
        // Set the button sprite to 'on'
        _spriteRenderer.sprite = _buttonOn;

        // Animate the barrier opening
        yield return AnimateBarrierOpening();

        // Wait for the open duration
        yield return new WaitForSeconds(_openDuration);

        // Reactivate the barrier GameObject before closing animation
        _barrier.SetActive(true);

        // Animate the barrier closing
        yield return AnimateBarrierClosing();

        // Set the button sprite back to 'off'
        _spriteRenderer.sprite = _buttonOff;
    }

    private IEnumerator AnimateBarrierOpening()
    {
        if(_barrierAudioSource != null) _barrierAudioSource.Play();
        Sprite[] openingSprites = { _barrier1, _barrier2, _barrier3, _barrier4, _barrier5 };
        foreach (var sprite in openingSprites)
        {
            _barrierSpriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f); // Adjust for animation speed
        }

        // Deactivate the barrier after it visually "opens"
        _barrier.SetActive(false);
    }

    private IEnumerator AnimateBarrierClosing()
    {
        if(_barrierAudioSource != null) _barrierAudioSource.Play();
        Sprite[] closingSprites = { _barrier5, _barrier4, _barrier3, _barrier2, _barrier1 };
        foreach (var sprite in closingSprites)
        {
            _barrierSpriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f); // Adjust for animation speed
        }
    }
}
