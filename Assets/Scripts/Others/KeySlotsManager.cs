using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeySlotsManager : MonoBehaviour
{
    [Header("Key Slot Images")]
    [SerializeField] private Image[] keySlots;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite _wKeySprite;
    [SerializeField] private Sprite _aKeySprite;
    [SerializeField] private Sprite _sKeySprite;
    [SerializeField] private Sprite _dKeySprite;
    [SerializeField] private Sprite _spaceKeySprite;
    [SerializeField] private Sprite _emptyKeySprite;

    private GameInputManager _inputManager;

    private void Start()
    {
        _inputManager = GameInputManager.Instance;
        if (_inputManager == null)
        {
            Debug.LogError("GameInputManager.Instance is null. Make sure it is properly initialized.");
        }
    }

    private void Update()
    {
        UpdateKeySlots();
    }

    private void UpdateKeySlots()
    {
        if (_inputManager == null) return;

        List<Sprite> activeButtons = GetActiveButtons();

        for (int i = 0; i < keySlots.Length; i++)
        {
            keySlots[i].sprite = i < activeButtons.Count ? activeButtons[i] : _emptyKeySprite;
        }
    }

    private List<Sprite> GetActiveButtons()
    {
        List<Sprite> activeButtons = new List<Sprite>();

        if (_inputManager.buttonW) activeButtons.Add(_wKeySprite);
        if (_inputManager.buttonA) activeButtons.Add(_aKeySprite);
        if (_inputManager.buttonS) activeButtons.Add(_sKeySprite);
        if (_inputManager.buttonD) activeButtons.Add(_dKeySprite);
        if (_inputManager.buttonSpace) activeButtons.Add(_spaceKeySprite);

        return activeButtons;
    }
}
