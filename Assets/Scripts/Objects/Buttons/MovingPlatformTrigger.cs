using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatformTrigger : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    private SpriteRenderer _buttonSpriteRenderer;

    [Header("Platform Settings")]
    [SerializeField] private Transform platform;
    [SerializeField] private Vector3 moveDirection = Vector3.up;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;
    private Vector3 initialPosition;
    private Coroutine moveCoroutine;

    private HashSet<Collider2D> objectsInTrigger = new();

    private void Awake()
    {
        _buttonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (platform != null)
        {
            initialPosition = platform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            if (!gameObject.activeInHierarchy) return;

            objectsInTrigger.Add(other);

            if (objectsInTrigger.Count > 1) return;

            if (_buttonSpriteRenderer != null) _buttonSpriteRenderer.sprite = _buttonOn;
            moveCoroutine = StartCoroutine(MovePlatform());
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
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator MovePlatform()
    {
        int completedRepetitions = 0;

        while (true)
        {
            Vector3 targetPosition = initialPosition + moveDirection * moveDistance;
            while (Vector3.Distance(platform.position, targetPosition) != 0f)
            {
                platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            while (Vector3.Distance(platform.position, initialPosition) != 0f)
            {
                platform.position = Vector3.MoveTowards(platform.position, initialPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            completedRepetitions++;
        }
    }
}
