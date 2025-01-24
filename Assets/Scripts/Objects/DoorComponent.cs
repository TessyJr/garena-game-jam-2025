using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerKeyComponent key = collision.gameObject.GetComponent<PlayerKeyComponent>();
            if(key._keyObtained == true){
                key.DropKey();
                Destroy(gameObject);
            }
        }
    }
}