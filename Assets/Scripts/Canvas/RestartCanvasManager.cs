using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartCanvasManager : MonoBehaviour
{
    [SerializeField] Button _restartButton;

    void Start()
    {
        _restartButton.onClick.AddListener(ReloadScene);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
