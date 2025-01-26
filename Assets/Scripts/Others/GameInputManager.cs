using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;

    [Header("Active Buttons")]
    public bool buttonA = false;
    public bool buttonD = false;
    public bool buttonW = false;
    public bool buttonS = false;
    public bool buttonSpace = false;

    private int activeButtonCount = 0;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>(); 
    }

    void Start()
    {
        UpdateActiveButtonCount();
    }

    // Optional: Add methods to enable/disable specific buttons
    public void SetButtonState(string button, bool state)
    {
        if (state)
        {
            // If we want to activate a button, we need to ensure only 2 buttons are activated at a time
            if (activeButtonCount >= 2 && !IsButtonActive(button))
            {
                Debug.LogWarning("Cannot activate more than two buttons at once.");
                return; // Don't activate more than two buttons
            }else{
                audioSource.Play(); 
            }
        }
        switch (button.ToUpper())
        {
            case "A":
                buttonA = state;
                break;
            case "D":
                buttonD = state;
                break;
            case "W":
                buttonW = state;
                break;
            case "S":
                buttonS = state;
                break;
            case "SPACE":
                buttonSpace = state;
                break;
        }

        // Update activeButtonCount after the change
        UpdateActiveButtonCount();
    }

    // Helper method to check if a button is already active
    public bool IsButtonActive(string button)
    {
        return button.ToUpper() switch
        {
            "A" => buttonA,
            "D" => buttonD,
            "W" => buttonW,
            "S" => buttonS,
            "SPACE" => buttonSpace,
            _ => false,
        };
    }

    private void UpdateActiveButtonCount()
    {
        activeButtonCount = 0;

        if (buttonA) activeButtonCount++;
        if (buttonD) activeButtonCount++;
        if (buttonW) activeButtonCount++;
        if (buttonS) activeButtonCount++;
        if (buttonSpace) activeButtonCount++;
    }

    public bool Is2ButtonsActive()
    {
        return activeButtonCount == 2;
    }
}
