using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleporterComponent : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Trigger activated by: {other.name}. Loading scene: {_sceneName}");

            SceneManager.LoadScene(_sceneName);
        }
    }
}
