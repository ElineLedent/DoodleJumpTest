using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _gameSoundsAudioSource = default;

    [SerializeField]
    private AudioSource _backGroundAudioSource = default;

    [SerializeField]
    private AudioClip _gameStartAudioClip = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _gameStartVolume = 1f;

    [SerializeField]
    private AudioClip _gameOverAudioClip = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _gameOverVolume = 1f;

    [SerializeField]
    private AudioClip _countdownAudioClip = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _countdownVolume = 1f;

    [SerializeField]
    private AudioClip _uiConfirmationAudioClip = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _uiConfirmationVolume = 1f;

    [SerializeField]
    private AudioClip _menuMusicAudioClip = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _menuMusicVolume = 1f;

    [SerializeField]
    private AudioClip _gameBackgroundAudioClip = default;

    [Range(0f, 1f)]
    [SerializeField]
    private float _gameBackgroundVolume = 1f;

    public void PlayAudioGameStart()
    {
        PlayAudio(_gameStartAudioClip, _gameStartVolume);
    }

    public void PlayAudioGameOver()
    {
        PlayAudio(_gameOverAudioClip, _gameOverVolume);
    }

    public void PlayAudioCountdown()
    {
        PlayAudio(_countdownAudioClip, _countdownVolume);
    }

    public void PlayAudioUIConfirmation()
    {
        PlayAudio(_uiConfirmationAudioClip, _uiConfirmationVolume);
    }

    public void PlayAudioMenuMusic()
    {
        PlayBackgroundAudio(_menuMusicAudioClip, _menuMusicVolume);
    }

    public void PlayAudioGameBackground()
    {
        PlayBackgroundAudio(_gameBackgroundAudioClip, _gameBackgroundVolume);
    }

    private void PlayAudio(AudioClip clip, float volume)
    {
        _gameSoundsAudioSource.clip = clip;
        _gameSoundsAudioSource.volume = volume;
        _gameSoundsAudioSource.Play();
    }

    private void PlayBackgroundAudio(AudioClip clip, float volume)
    {
        _backGroundAudioSource.clip = clip;
        _backGroundAudioSource.volume = volume;
        _backGroundAudioSource.Play();
    }

    private void Awake()
    {
        Debug.Assert(_gameSoundsAudioSource != null, "Missing reference!");
        Debug.Assert(_backGroundAudioSource != null, "Missing reference!");

        Debug.Assert(_gameStartAudioClip != null, "Missing reference!");
        Debug.Assert(_gameOverAudioClip != null, "Missing reference!");
        Debug.Assert(_countdownAudioClip != null, "Missing reference!");
        Debug.Assert(_uiConfirmationAudioClip != null, "Missing reference!");
        Debug.Assert(_menuMusicAudioClip != null, "Missing reference!");
        Debug.Assert(_gameBackgroundAudioClip != null, "Missing reference!");
    }
}