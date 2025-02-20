using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;

    [Header("Active Buttons")]
    public bool buttonA = false;
    public bool buttonD = false;
    public bool buttonW = false;
    public bool buttonS = false;
    public bool buttonSPACE = false;

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

    public void SetButtonState(string button, bool state)
    {
        if (state)
        {
            if (activeButtonCount >= 2 && !IsButtonActive(button))
            {
                return;
            }
            else
            {
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
                buttonSPACE = state;
                break;
        }

        UpdateActiveButtonCount();
    }

    public bool IsButtonActive(string button)
    {
        return button.ToUpper() switch
        {
            "A" => buttonA,
            "D" => buttonD,
            "W" => buttonW,
            "S" => buttonS,
            "SPACE" => buttonSPACE,
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
        if (buttonSPACE) activeButtonCount++;
    }

    public bool Is2ButtonsActive()
    {
        return activeButtonCount == 2;
    }
}
