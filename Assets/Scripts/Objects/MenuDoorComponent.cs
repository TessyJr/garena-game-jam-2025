using UnityEngine;

public class MenuDoorComponent : MonoBehaviour
{
    [SerializeField] private Animator _door;
    [SerializeField] private AudioSource _unlockedSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                _door.SetBool("isOpened", true);
                _unlockedSound.Play();
        }
    }
        private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                _door.SetBool("isOpened", false);
                _unlockedSound.Play();
        }
    }
}