using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenBarrierTrigger : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    [SerializeField] private AudioSource _barrierAudioSource;
    private SpriteRenderer _buttonSpriteRenderer;

    [Header("Barrier Settings")]
    [SerializeField] private GameObject _barrier;
    [SerializeField] private Sprite[] _barrierSprites;
    private SpriteRenderer _barrierSpriteRenderer;
    private BoxCollider2D _barrierBoxCollider;
    private Coroutine _barrierCoroutine;
    private int _barrierSpriteNumber = 0;

    private HashSet<Collider2D> objectsInTrigger = new();

    private void Awake()
    {
        _buttonSpriteRenderer = GetComponent<SpriteRenderer>();
        _barrierSpriteRenderer = _barrier.GetComponent<SpriteRenderer>();
        _barrierBoxCollider = _barrier.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            if (!gameObject.activeInHierarchy) return;

            objectsInTrigger.Add(other);

            if (objectsInTrigger.Count > 1) return;

            if (_buttonSpriteRenderer != null) _buttonSpriteRenderer.sprite = _buttonOn;
            if (_barrierCoroutine != null) StopCoroutine(_barrierCoroutine);
            _barrierCoroutine = StartCoroutine(IncreaseBarrierSpriteNumber());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            if (!gameObject.activeInHierarchy) return;

            objectsInTrigger.Remove(other);

            if (objectsInTrigger.Count > 0) return;

            if (_buttonSpriteRenderer != null) _buttonSpriteRenderer.sprite = _buttonOff;
            if (_barrierCoroutine != null) StopCoroutine(_barrierCoroutine);
            _barrierCoroutine = StartCoroutine(DecreaseBarrierSpriteNumber());
        }
    }

    private IEnumerator IncreaseBarrierSpriteNumber()
    {
        while (_barrierSpriteNumber < 4)
        {
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
