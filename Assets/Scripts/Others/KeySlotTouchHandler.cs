
using UnityEngine;
using UnityEngine.EventSystems;

public class KeySlotTouchHandler : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private KeySlotsManager _keySlotsManager;
    [SerializeField] private PlayerInputComponent _playerInput;
    [SerializeField] private PlayerMovementComponent _playerMovement;

    [Header("Settings")]
    [SerializeField] private int _slotIndex;
    [SerializeField] private float _swipeThreshold = 80f;

    private Vector2 _pointerDownPosition;
    private string _currentButton;
    private bool _isPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownPosition = eventData.position;

        _currentButton = _keySlotsManager.GetActiveButtonName(_slotIndex);

        if (string.IsNullOrEmpty(_currentButton))
            return;

        _isPressed = true;

        // Start continuous movement / climbing.
        // if (_currentButton != "SPACE" && _currentButton != "S")
        // {
            _playerMovement.SetMobileInput(_currentButton, true);
        // }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isPressed || string.IsNullOrEmpty(_currentButton))
            return;

        // Normal release.
        if (_currentButton == "SPACE" || _currentButton == "S")
        {
            _playerMovement.SetMobileInput(_currentButton, true);
        }

        _playerMovement.SetMobileInput(_currentButton, false);

        _isPressed = false;
        _currentButton = null;
    }

    public void OnDiscardButtonClicked()
    {
        string button = _keySlotsManager.GetActiveButtonName(_slotIndex);

        if (string.IsNullOrEmpty(button))
            return;

        _playerMovement.SetMobileInput(button, false);
        _playerInput.DiscardKeySlot(button);
    }
}
