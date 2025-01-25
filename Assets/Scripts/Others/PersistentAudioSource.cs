using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudioSource : MonoBehaviour
{
    private AudioSource _audioSource;
    private static PersistentAudioSource instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        FetchAndPlayBgm();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FetchAndPlayBgm();
    }

    private void FetchAndPlayBgm()
    {
        GameObject sceneBgmObject = GameObject.FindWithTag("SceneBgm");

        if (sceneBgmObject == null)
        {
            Debug.LogWarning("No object tagged 'SceneBgm' found in the current scene.");
            return;
        }

        AudioSource sceneBgmSource = sceneBgmObject.GetComponent<AudioSource>();
        AudioClip sceneBgmClip = sceneBgmSource.clip;
        // _audioSource.volume = sceneBgmSource.volume;

        if (sceneBgmClip == null)
        {
            Debug.LogWarning("SceneBgm does not have an AudioClip assigned.");
            return;
        }

        // Debug.Log($"Current AudioClip: {_audioSource.clip}, New Scene AudioClip: {sceneBgmClip}");

        if (_audioSource.clip != sceneBgmClip)
        {
            _audioSource.clip = sceneBgmClip;
            _audioSource.Play();
            // Debug.Log("New BGM set and playing.");
        }
        else
        {
            // Debug.Log("BGM is already playing the correct clip.");
        }
    }
}