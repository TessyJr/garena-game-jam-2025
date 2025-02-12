using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonComponent : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button button;
    [SerializeField] private PlayerMovementComponent _playerMovement;
    private float _movementAmount = 16f;
    private float _movementTime = 2f;

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(ChangeScene);
        }
    }

    private void ChangeScene()
    {
        // Start the auto-move routine (it can be a coroutine).
        StartCoroutine(_playerMovement.AutoMoveLeft(_movementAmount, _movementTime));

        // Schedule the scene load after _movementTime seconds without using a coroutine.
        Invoke(nameof(LoadSceneAfterMovement), _movementTime);
    }

    private void LoadSceneAfterMovement()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(ChangeScene);
        }
    }
}
