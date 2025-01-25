using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatformTrigger : MonoBehaviour
{
    [SerializeField] private Transform platform; // The platform to move
    [SerializeField] private Vector3 moveDirection = Vector3.up; // Direction to move the platform
    [SerializeField] private float moveDistance = 2f; // Distance to move in the direction
    [SerializeField] private float moveSpeed = 2f; // Speed of movement
    [SerializeField] private int backForth = 2; // Number of back-and-forth movements

    private Vector3 initialPosition; // The platform's starting position
    private Coroutine moveCoroutine; // Reference to the running coroutine

    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;

    private SpriteRenderer _spriteRenderer;

    private HashSet<Collider2D> objectsInTrigger = new HashSet<Collider2D>(); // Track objects in the trigger

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (platform != null)
        {
            initialPosition = platform.position;
        }
    }

    private void OnDestroy()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            objectsInTrigger.Add(other); // Add object to the set
            UpdateButtonState();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            objectsInTrigger.Remove(other); // Remove object from the set
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        if (objectsInTrigger.Count > 0)
        {
            _spriteRenderer.sprite = _buttonOn;

            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MovePlatform(-1)); // Infinite back-and-forth
            }
        }
        else
        {
            _spriteRenderer.sprite = _buttonOff;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
    }

    private IEnumerator MovePlatform(int repetitions)
    {
        int completedRepetitions = 0;

        while (repetitions < 0 || completedRepetitions < repetitions)
        {
            // Move to the target position
            Vector3 targetPosition = initialPosition + moveDirection * moveDistance;
            while (Vector3.Distance(platform.position, targetPosition) > 0.01f)
            {
                platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Move back to the initial position
            while (Vector3.Distance(platform.position, initialPosition) > 0.01f)
            {
                platform.position = Vector3.MoveTowards(platform.position, initialPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            completedRepetitions++;

            if (repetitions > 0 && completedRepetitions >= repetitions && objectsInTrigger.Count == 0)
            {
                break;
            }
        }

        moveCoroutine = null; // Clear reference when the coroutine finishes
    }
}
