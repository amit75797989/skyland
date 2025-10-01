using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance { get; private set; }

    [Header("Audio Source Setup")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Play a one-shot sound effect
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// Play or change background music
    /// </summary>
    public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (clip == null || (musicSource.clip == clip && musicSource.isPlaying)) return;

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    /// <summary>
    /// Stop music playback
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Toggle mute
    /// </summary>
    public void MuteAll(bool isMuted)
    {
        sfxSource.mute = isMuted;
        musicSource.mute = isMuted;
    }

    /// <summary>
    /// Toggle mute SFX
    /// </summary>
    public void MuteSFX(bool isMuted)
    {
        sfxSource.mute = isMuted;
    }


    /// <summary>
    /// Toggle mute Music
    /// </summary>
    public void MuteMusic(bool isMuted)
    {
        musicSource.mute = isMuted;
    }
}
