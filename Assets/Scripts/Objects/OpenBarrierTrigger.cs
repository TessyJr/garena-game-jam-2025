using UnityEngine;
using System.Collections;

public class OpenBarrierTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _barrier;
    [SerializeField] private float _openDuration;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(_barrier.activeSelf == true) 
            {
                _barrier.SetActive(false);
                StartCoroutine(CloseBarrier());
            }
        }
    }

    private IEnumerator CloseBarrier(){
        yield return new WaitForSeconds(_openDuration);
        _barrier.SetActive(true);
    }
}