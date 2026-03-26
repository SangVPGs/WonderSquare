using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Music")]
    public AudioClip[] musicTrack;

    [Header("Game Sounds")]
    public AudioClip move;
    public AudioClip rotate;
    public AudioClip hardDrop;
    public AudioClip hold;
    public AudioClip lineClear;
    public AudioClip highLineClear;
    public AudioClip[] countdown;
    public AudioClip gameOver;
    public AudioClip button;

    private int currentTrackIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplySettings();
    }

    // ================= SFX =================

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMove() => PlaySFX(move);
    public void PlayRotate() => PlaySFX(rotate);
    public void PlayHardDrop() => PlaySFX(hardDrop);
    public void PlayHold() => PlaySFX(hold);
    public void PlayLineClear() => PlaySFX(lineClear);
    public void PlayHighLineClear() => PlaySFX(highLineClear);
    public void PlayCountdown(int index)
    {
        if (index < 0 || index >= countdown.Length) return;
        PlaySFX(countdown[index]);
    }
    public void PlayGameOver() => PlaySFX(gameOver);
    public void PlayButton() => PlaySFX(button);

    // ================= VOLUME (MIXER) =================

    public void SetMusicVolume(float value)
    {
        value = Mathf.Max(value, 0.0001f);

        float baseDb = -5f; // Value cho mixer
        float userDb = Mathf.Log10(value) * 20; // Value cho slider trong setting

        mixer.SetFloat("MusicVolume", baseDb + userDb);
    }

    public void SetSFXVolume(float value)
    {
        value = Mathf.Max(value, 0.0001f);

        float baseDb = 0f;
        float userDb = Mathf.Log10(value) * 20;

        mixer.SetFloat("SFXVolume", baseDb + userDb);
    }

    // ================= APPLY =================

    public void ApplySettings()
    {
        SetMusicVolume(SettingsManager.MusicVolume);
        SetSFXVolume(SettingsManager.SfxVolume);

        PlayMusic(SettingsManager.MusicIndex);
    }

    // ================= MUSIC =================

    public void PlayMusic(int index)
    {
        if (musicTrack.Length == 0) return;
        if (index < 0 || index >= musicTrack.Length) return;
        if (currentTrackIndex == index) return;

        currentTrackIndex = index;

        musicSource.clip = musicTrack[index];
        musicSource.loop = true;
        musicSource.Play();
    }
}