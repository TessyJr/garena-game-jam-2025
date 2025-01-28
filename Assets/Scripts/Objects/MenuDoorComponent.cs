using System.Collections;
using UnityEngine;

public class MenuDoorComponent : MonoBehaviour
{
    [SerializeField] private Animator _door;
    [SerializeField] private BoxCollider2D _doorCollider;
    [SerializeField] private AudioSource _unlockedSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _door.SetBool("isOpened", true);
            _unlockedSound.Play();

            StartCoroutine(DisableDoorColliderWithDelay());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _doorCollider.enabled = true;
            _door.SetBool("isOpened", false);
            _unlockedSound.Play();
        }
    }

    private IEnumerator DisableDoorColliderWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        _doorCollider.enabled = false;
    }
}
