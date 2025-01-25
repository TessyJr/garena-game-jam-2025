using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonComponent : MonoBehaviour
{
    [SerializeField] private string sceneName; // The name of the scene to load
    [SerializeField] private Button button;   // Reference to the button component

    private void Start()
    {
        // Ensure the button is assigned and subscribe to its click event
        if (button != null)
        {
            button.onClick.AddListener(ChangeScene);
        }
        else
        {
            Debug.LogWarning("Button is not assigned in the inspector.");
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not assigned or is empty.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the button click event to prevent potential memory leaks
        if (button != null)
        {
            button.onClick.RemoveListener(ChangeScene);
        }
    }
}
