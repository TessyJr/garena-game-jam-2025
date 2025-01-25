using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.EventSystems;

public class MenuCanvasManager : MonoBehaviour
{
    [Header("Restart Settings")]
    [SerializeField] private Button _restartButton;

    [Header("Spectate Settings")]
    [SerializeField] private Button _spectateButton;
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] private float cameraMoveSpeed = 8f;
    [SerializeField] private float edgeThreshold = 64f; // Distance from the edge of the screen to trigger movement
    public bool _isSpectating = false;

    [Header("Home Settings")]
    [SerializeField] private Button _homeButton;

    void Start()
    {
        _restartButton.onClick.AddListener(() =>
        {
            ReloadScene();
            ClearButtonFocus();
        });

        _spectateButton.onClick.AddListener(() =>
        {
            ToggleSpectateMode();
            ClearButtonFocus();
        });
        _homeButton.onClick.AddListener(() =>
        {
            HomeScene();
            ClearButtonFocus();
        });
    }
    void Update()
    {
        if (_isSpectating)
        {
            HandleCameraMovement();
        }
    }

    void ReloadScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HomeScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("MenuScene");
    }

    void ToggleSpectateMode()
    {
        _isSpectating = !_isSpectating;

        if (_isSpectating)
        {
            // Detach the camera from the player
            _cinemachineCamera.Follow = null;
        }
        else
        {
            // Reattach the camera to the player
            _cinemachineCamera.Follow = _player.transform;
        }
    }

    private void HandleCameraMovement()
    {
        Vector3 newPosition = _cinemachineCamera.transform.position;

        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Check if the mouse is near the edges of the screen
        if (mousePosition.x < edgeThreshold) // Near the left edge
        {
            newPosition.x -= cameraMoveSpeed * Time.deltaTime;
        }
        if (mousePosition.x > Screen.width - edgeThreshold) // Near the right edge
        {
            newPosition.x += cameraMoveSpeed * Time.deltaTime;
        }
        if (mousePosition.y < edgeThreshold) // Near the bottom edge
        {
            newPosition.y -= cameraMoveSpeed * Time.deltaTime;
        }
        if (mousePosition.y > Screen.height - edgeThreshold) // Near the top edge
        {
            newPosition.y += cameraMoveSpeed * Time.deltaTime;
        }

        // Apply the new position to the camera
        _cinemachineCamera.transform.position = newPosition;
    }

    private void ClearButtonFocus()
    {
        // Clear the currently selected object in the EventSystem
        EventSystem.current.SetSelectedGameObject(null);
    }
}
