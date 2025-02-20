using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleporterComponent : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
