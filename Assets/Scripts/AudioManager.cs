using UnityEngine;

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip rewindWarningClip;
    public AudioClip rewindClip;
    private AudioSource audioSource;
    public AudioClip doorMoveClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // maybe??? If no Boom when scene change(___:(____)
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string soundName)
    {
        switch (soundName)
        {
            case "TimeRewindWarning":
                audioSource.PlayOneShot(rewindWarningClip);
                Debug.Log("WARNING I AM GOING TO REWIND");
                break;
            case "TimeRewind":
                audioSource.PlayOneShot(rewindClip);
                Debug.Log("REWINDING REWINDING");
                break;
            case "doorMove":
                audioSource.PlayOneShot(doorMoveClip);
                break;
        }
    }
}

