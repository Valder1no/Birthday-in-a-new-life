using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip rewindWarningClip;
    public AudioClip rewindClip;
    public AudioClip doorMoveClip;
    public AudioClip Line1;
    public AudioClip Line2;
    public AudioClip Line3;
    public AudioClip Line4;
    public AudioClip Line5;
    public AudioClip Line6;
    public AudioClip Line7;
    public AudioClip Line8;
    public AudioClip Line9;
    public AudioClip Line10;
    public AudioClip hugoEasterEgg;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // 🔊 Utility method for reusability
    public void PlayClip(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    // ✅ Individual methods for each sound
    public void PlayRewindWarning() => PlayClip(rewindWarningClip);
    public void PlayRewind() => PlayClip(rewindClip);
    public void PlayDoorMove() => PlayClip(doorMoveClip);

    public void PlayLine1() { PlayClip(Line1); Debug.Log("PLAYING CLIP !"); }
    public void PlayLine2() => PlayClip(Line2);
    public void PlayLine3() => PlayClip(Line3);
    public void PlayLine4() => PlayClip(Line4);
    public void PlayLine5() => PlayClip(Line5);
    public void PlayLine6() => PlayClip(Line6);
    public void PlayLine7() => PlayClip(Line7);
    public void PlayLine8() => PlayClip(Line8);
    public void PlayLine9() => PlayClip(Line9);
    public void PlayLine10() { PlayClip(Line10); SceneManager.LoadScene(0);}
    public void PlayEasterEgg() => PlayClip(hugoEasterEgg);
}
