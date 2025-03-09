using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("Audio Clips")]

    public AudioClip bg;
    public AudioClip shoot;
    public AudioClip death;
    public AudioClip hit;
    public AudioClip jump;
    public AudioClip dash;

    private void Start() {
        musicSource.clip = bg;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) {
        SFXSource.PlayOneShot(clip);
    }

     public void PlaySFXVol(AudioClip clip, float vol) {
        SFXSource.PlayOneShot(clip, vol);
    }

}
