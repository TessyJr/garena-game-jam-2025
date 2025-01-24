using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySlotsManager : MonoBehaviour
{
    [Header("Key Slot Images")]
    [SerializeField] private GameObject keySlot1; // Assign the KeySlot1 GameObject
    [SerializeField] private GameObject keySlot2; // Assign the KeySlot2 GameObject

    private TextMeshProUGUI keySlot1Text;
    private TextMeshProUGUI keySlot2Text;

    private void Start()
    {
        // Get the TMP components from the child objects
        keySlot1Text = keySlot1.GetComponentInChildren<TextMeshProUGUI>();
        keySlot2Text = keySlot2.GetComponentInChildren<TextMeshProUGUI>();

        if (keySlot1Text == null || keySlot2Text == null)
        {
            Debug.LogError("Key slot TMP components not found. Ensure each key slot has a TMP component as a child.");
        }

        UpdateKeySlots();
    }

    private void Update()
    {
        // Continuously update the key slots if needed
        UpdateKeySlots();
    }

    private void UpdateKeySlots()
    {
        // Get the active buttons from GameInputManager
        GameInputManager inputManager = GameInputManager.Instance;
        if (inputManager == null) return;

        string[] activeButtons = GetActiveButtons(inputManager);

        // Update the TMP text for each key slot
        if (activeButtons.Length >= 1)
        {
            keySlot1Text.text = activeButtons[0];
            LayoutRebuilder.ForceRebuildLayoutImmediate(keySlot1Text.rectTransform);
        }
        else
        {
            keySlot1Text.text = ""; // Clear if no active buttons
        }

        if (activeButtons.Length >= 2)
        {
            keySlot2Text.text = activeButtons[1];
            LayoutRebuilder.ForceRebuildLayoutImmediate(keySlot2Text.rectTransform);
        }
        else
        {
            keySlot2Text.text = ""; // Clear if only one or no active buttons
        }
    }

    private string[] GetActiveButtons(GameInputManager inputManager)
    {
        // Collect active buttons
        var activeButtons = new System.Collections.Generic.List<string>();

        if (inputManager.buttonA) activeButtons.Add("A");
        if (inputManager.buttonD) activeButtons.Add("D");
        if (inputManager.buttonW) activeButtons.Add("W");
        if (inputManager.buttonSpace) activeButtons.Add("Sp");

        return activeButtons.ToArray();
    }
}
