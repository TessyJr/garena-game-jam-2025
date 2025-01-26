using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    [SerializeField] private Animator _door;
    [SerializeField] private BoxCollider2D _doorCollider;
    [SerializeField] private AudioSource _lockedSound;
    [SerializeField] private AudioSource _unlockedSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerKeyComponent key = other.gameObject.GetComponent<PlayerKeyComponent>();
            if (key != null && key._keyObtained == true)
            {
                _door.SetBool("isOpened", true);
                _unlockedSound.Play();
                key.DropKey();
                Destroy(_doorCollider);
            }else{
                _lockedSound.Play();
            }
        }
    }
}