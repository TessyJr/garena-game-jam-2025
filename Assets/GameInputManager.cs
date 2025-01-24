using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;

    [Header("Active Buttons")]
    public bool buttonA = true;
    public bool buttonD = true;
    public bool buttonSpace = true;

    private void Awake()
    {
        // Ensure there's only one instance of GameInputManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Optional: Add methods to enable/disable specific buttons
    public void SetButtonState(string button, bool state)
    {
        switch (button.ToUpper())
        {
            case "A":
                buttonA = state;
                break;
            case "D":
                buttonD = state;
                break;
            case "SPACE":
                buttonSpace = state;
                break;
        }
    }
}
