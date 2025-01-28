using UnityEngine;
using System.Collections;

public class OpenBarrierTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _barrier;

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

    private Coroutine _currentAnimationCoroutine;

    private int _triggerCount = 0; // Tracks the number of objects in the trigger zone
    private bool _isAnimating = false;
    private bool _shouldOpen = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _barrierSpriteRenderer = _barrier.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            _triggerCount++; // Increment the count when a valid object enters the trigger zone
            _shouldOpen = true;

            if (!_isAnimating)
            {
                StartOpeningBarrier();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            _triggerCount--; // Decrement the count when a valid object exits the trigger zone

            if (_triggerCount <= 0)
            {
                _triggerCount = 0; // Ensure the count doesn't go negative
                _shouldOpen = false;

                if (!_isAnimating)
                {
                    StartClosingBarrier();
                }
            }
        }
    }

    private void StartOpeningBarrier()
    {
        if (!gameObject.activeInHierarchy) return; // Ensure the GameObject is active
        if (_currentAnimationCoroutine != null)
        {
            StopCoroutine(_currentAnimationCoroutine);
        }

        _currentAnimationCoroutine = StartCoroutine(AnimateBarrierOpening());
    }

    private void StartClosingBarrier()
    {
        if (!gameObject.activeInHierarchy) return; // Ensure the GameObject is active
        if (_currentAnimationCoroutine != null)
        {
            StopCoroutine(_currentAnimationCoroutine);
        }

        _currentAnimationCoroutine = StartCoroutine(AnimateBarrierClosing());
    }

    private IEnumerator AnimateBarrierOpening()
    {
        _isAnimating = true;
        _spriteRenderer.sprite = _buttonOn;

        if (_barrierAudioSource != null) _barrierAudioSource.Play();
        Sprite[] openingSprites = { _barrier1, _barrier2, _barrier3, _barrier4, _barrier5 };

        foreach (var sprite in openingSprites)
        {
            _barrierSpriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f);

            // If `_shouldOpen` changes to false mid-animation, stop and close the barrier
            if (!_shouldOpen)
            {
                StartClosingBarrier();
                yield break;
            }
        }

        _barrier.SetActive(false);
        _isAnimating = false;
    }

    private IEnumerator AnimateBarrierClosing()
    {
        _isAnimating = true;
        _barrier.SetActive(true);

        if (_barrierAudioSource != null) _barrierAudioSource.Play();
        Sprite[] closingSprites = { _barrier5, _barrier4, _barrier3, _barrier2, _barrier1 };

        foreach (var sprite in closingSprites)
        {
            _barrierSpriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f);

            // If `_shouldOpen` changes to true mid-animation, stop and open the barrier
            if (_shouldOpen)
            {
                StartOpeningBarrier();
                yield break;
            }
        }

        _spriteRenderer.sprite = _buttonOff; // Reset button sprite to "off" after closing
        _isAnimating = false;
    }
}
