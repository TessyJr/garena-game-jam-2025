using UnityEngine;
using System.Collections;

public class OpenBarrierTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _barrier;
    [SerializeField] private GameObject _bottomBarrier;
    [SerializeField] private float _openDuration;

    [Header("Button Sprites")]
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    [SerializeField] private AudioSource _barrierAudioSource;

    [Header("Barrier Sprites")]
    [SerializeField] private Sprite _barrier1; // Closed
    [SerializeField] private Sprite _barrier2;
    [SerializeField] private Sprite _barrier3;
    [SerializeField] private Sprite _barrier4;
    [SerializeField] private Sprite _barrier5; // Opened

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _barrierSpriteRenderer;
    private SpriteRenderer _bottomBarrierSpriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _barrierSpriteRenderer = _barrier.GetComponent<SpriteRenderer>();
        _bottomBarrierSpriteRenderer = _bottomBarrier.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_barrier.activeSelf == true)
            {
                _spriteRenderer.sprite = _buttonOn;

                StartCoroutine(AnimateBarrierOpening(_barrierSpriteRenderer, _barrier));
                StartCoroutine(AnimateBarrierClosing(_bottomBarrierSpriteRenderer, _bottomBarrier));

                _spriteRenderer.sprite = _buttonOff;
            }else{
                _spriteRenderer.sprite = _buttonOn;
                
                StartCoroutine(AnimateBarrierOpening(_bottomBarrierSpriteRenderer, _bottomBarrier));
                StartCoroutine(AnimateBarrierClosing(_barrierSpriteRenderer, _barrier));
                
                _spriteRenderer.sprite = _buttonOff;
            }
        }
    }


    private IEnumerator AnimateBarrierOpening(SpriteRenderer spriteRenderer, GameObject barrierObject)
    {
        if(_barrierAudioSource != null) _barrierAudioSource.Play();
        Sprite[] openingSprites = { _barrier1, _barrier2, _barrier3, _barrier4, _barrier5 };
        foreach (var sprite in openingSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f);
        }
        barrierObject.SetActive(false);
    }

    private IEnumerator AnimateBarrierClosing(SpriteRenderer spriteRenderer, GameObject barrierObject)
    {
        barrierObject.SetActive(true);
        if(_barrierAudioSource != null) _barrierAudioSource.Play();
        Sprite[] closingSprites = { _barrier5, _barrier4, _barrier3, _barrier2, _barrier1 };
        foreach (var sprite in closingSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
