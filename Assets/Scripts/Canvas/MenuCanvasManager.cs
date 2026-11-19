using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;

public class MenuCanvasManager : MonoBehaviour
{
    [Header("Restart Settings")]
    [SerializeField] private Button _restartButton;

    [Header("Spectate Settings")]
    [SerializeField] private Button _spectateButton;
    [SerializeField] private Unity.Cinemachine.CinemachineVirtualCamera _cinemachineCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] private float cameraMoveSpeed = 8f;
    [SerializeField] private float edgeThreshold = 64f; // Distance from the edge of the screen to trigger movement
    public bool _isSpectating = false;
    [SerializeField] private GameObject _spectatorModeText;

    private Vector2 _lastTouchPosition;

    [Header("Home Settings")]
    [SerializeField] private Button _homeButton;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
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

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadScene();
            }
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
            _spectatorModeText.SetActive(true);
            ChangeButtonColor(_spectateButton, new Color(0.81f, 0.53f, 0.25f)); // Orange (Hex: CF883F)

            // Detach the camera from the player
            _cinemachineCamera.Follow = null;
        }
        else
        {
            _spectatorModeText.SetActive(false);
            ChangeButtonColor(_spectateButton, Color.white); // White

            // Reattach the camera to the player
            _cinemachineCamera.Follow = _player.transform;
        }
    }


    private void HandleCameraMovement()
    {
        Vector3 newPosition = _cinemachineCamera.transform.position;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 currentWorldPosition = _mainCamera.ScreenToWorldPoint(
                    new Vector3(touch.position.x, touch.position.y, _mainCamera.nearClipPlane)
                );

                Vector3 lastWorldPosition = _mainCamera.ScreenToWorldPoint(
                    new Vector3(_lastTouchPosition.x, _lastTouchPosition.y, _mainCamera.nearClipPlane)
                );

                Vector3 worldDelta = currentWorldPosition - lastWorldPosition;

                newPosition -= worldDelta;

                _lastTouchPosition = touch.position;
            }
        }
        else
        {
            Vector3 mousePosition = Input.mousePosition;

            if (mousePosition.x < edgeThreshold)
                newPosition.x -= cameraMoveSpeed * Time.deltaTime;

            if (mousePosition.x > Screen.width - edgeThreshold)
                newPosition.x += cameraMoveSpeed * Time.deltaTime;

            if (mousePosition.y < edgeThreshold)
                newPosition.y -= cameraMoveSpeed * Time.deltaTime;

            if (mousePosition.y > Screen.height - edgeThreshold)
                newPosition.y += cameraMoveSpeed * Time.deltaTime;
        }

        _cinemachineCamera.transform.position = newPosition;
    }

    private void ClearButtonFocus()
    {
        // Clear the currently selected object in the EventSystem
        EventSystem.current.SetSelectedGameObject(null);
    }

    void ChangeButtonColor(Button button, Color targetColor)
    {
        var colors = button.colors;
        colors.normalColor = targetColor; // Set the normal color
        button.colors = colors;
    }

}
