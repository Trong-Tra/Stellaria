using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip Game_Music;

    void Start()
    {
        musicSource.clip = Game_Music;
        musicSource.Play();
    }
}
