using UnityEngine;
using System.Collections;

public class OpenBarrierTrigger : MonoBehaviour
{
    [Header("Button Settigns")]
    [SerializeField] private SpriteRenderer _buttonSpriteRenderer;
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    [SerializeField] private AudioSource _barrierAudioSource;

    [Header("Barrier Settings")]
    [SerializeField] private SpriteRenderer _barrierSpriteRenderer;
    [SerializeField] private BoxCollider2D _barrierBoxCollider;
    [SerializeField] private Sprite[] _barrierSprites;
    private int _barrierSpriteNumber = 0;
    private Coroutine _barrierCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            if (!gameObject.activeInHierarchy) return; // Prevent running if inactive
            if (_barrierCoroutine != null) StopCoroutine(_barrierCoroutine);
            _barrierCoroutine = StartCoroutine(IncreaseBarrierSpriteNumber());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            if (!gameObject.activeInHierarchy) return; // Prevent running if inactive
            if (_barrierCoroutine != null) StopCoroutine(_barrierCoroutine);
            _barrierCoroutine = StartCoroutine(DecreaseBarrierSpriteNumber());
        }
    }

    private IEnumerator IncreaseBarrierSpriteNumber()
    {
        while (_barrierSpriteNumber < 4)
        {
            _buttonSpriteRenderer.sprite = _buttonOn;
            _barrierAudioSource.Play();
            _barrierSpriteNumber++;
            _barrierSpriteRenderer.sprite = _barrierSprites[_barrierSpriteNumber];

            UpdateBarrierBoxCollider();

            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator DecreaseBarrierSpriteNumber()
    {
        while (_barrierSpriteNumber > 0)
        {
            _buttonSpriteRenderer.sprite = _buttonOff;
            _barrierAudioSource.Play();
            _barrierSpriteNumber--;
            _barrierSpriteRenderer.sprite = _barrierSprites[_barrierSpriteNumber];

            UpdateBarrierBoxCollider();

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void UpdateBarrierBoxCollider()
    {
        if (_barrierSpriteNumber == 4)
        {
            _barrierBoxCollider.enabled = false;
        }
        else
        {
            _barrierBoxCollider.enabled = true;
        }
    }
}
