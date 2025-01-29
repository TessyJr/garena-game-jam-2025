using UnityEngine;

public class PlayerStateComponent : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState = PlayerState.Idle;
    [SerializeField] private Animator _animator;

    public void SetState(PlayerState playerState)
    {
        _playerState = playerState;

        switch (_playerState)
        {
            case PlayerState.Idle:
                _animator.SetTrigger("idle");
                break;
            case PlayerState.Walking:
                _animator.SetTrigger("walk");
                break;
            case PlayerState.Climbing:
                _animator.SetTrigger("climb");
                break;
            case PlayerState.Jumping:
                _animator.SetTrigger("jump");
                break;
        }
    }
}
