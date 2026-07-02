using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfx;
    public AudioSource ambientMusic;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfx != null && clip != null)
        {
            sfx.PlayOneShot(clip);
        }
    }
}
