using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTeleportPipeComponent : MonoBehaviour
{
    [Header("Pipe Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask pipeLayer;
    [SerializeField] private PipeComponent currentPipe;

    void Update()
    {
        CheckPipe();
        HandleTeleport();
    }

    private void CheckPipe()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, pipeLayer);

        if (colliders.Length > 0)
        {
            currentPipe = colliders[0].gameObject.GetComponentInParent<PipeComponent>();
        }
        else
        {
            currentPipe = null;
        }
    }

    private void HandleTeleport()
    {
        if (currentPipe != null)
        {
            print($"Standing on: {currentPipe.name}");

            if (GameInputManager.Instance.buttonS && Input.GetKeyDown(KeyCode.S))
            {
                print(1);
                TeleportToOtherPipe(currentPipe);
            }
        }
    }

    private void TeleportToOtherPipe(PipeComponent currentPipe)
    {
        if (currentPipe != null && currentPipe.LinkedPipe != null)
        {
            // Teleport the player to the linked pipe
            transform.position = currentPipe.LinkedPipe.GetChild(0).position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
