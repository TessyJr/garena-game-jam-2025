using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RestartCanvasManager : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    void Start()
    {
        _restartButton.onClick.AddListener(ReloadScene);
    }

    void ReloadScene()
    {
        // Stop all coroutines and ensure no lingering references to destroyed objects
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
