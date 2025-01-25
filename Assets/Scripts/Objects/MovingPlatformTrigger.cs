using UnityEngine;
using System.Collections;

public class MovingPlatformTrigger : MonoBehaviour
{
    [SerializeField] private Transform platform; // The platform to move
    [SerializeField] private Vector3 moveDirection = Vector3.up; // Direction to move the platform
    [SerializeField] private float moveDistance = 2f; // Distance to move in the direction
    [SerializeField] private float moveSpeed = 2f; // Speed of movement
    [SerializeField] private int backForth = 2; // Number of back-and-forth movements

    private Vector3 initialPosition; // The platform's starting position
    private bool isPlayerInTrigger = false; // Whether the player is in the trigger zone
    private Coroutine moveCoroutine; // Reference to the running coroutine

    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;

    private SpriteRenderer _spriteRenderer;

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
        // Stop coroutines when the object is destroyed
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
            _spriteRenderer.sprite = _buttonOn;
            isPlayerInTrigger = true;

            // Stop any existing coroutine and start a new one
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MovePlatform(backForth));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            isPlayerInTrigger = true;

            // Ensure infinite movement while the player stays in the trigger
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MovePlatform(-1)); // Infinite back-and-forth
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ButtonObject"))
        {
            _spriteRenderer.sprite = _buttonOff;
            isPlayerInTrigger = false;

            // Stop any existing coroutine and start a new one with finite movement
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                // moveCoroutine = StartCoroutine(MovePlatform(backForth));
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

            // Exit loop if repetitions are complete and the player is not in the trigger
            if (repetitions > 0 && completedRepetitions >= repetitions && !isPlayerInTrigger)
            {
                break;
            }
        }

        moveCoroutine = null; // Clear reference when the coroutine finishes
    }

}
