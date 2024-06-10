using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : Singleton<SettingsMenu>
{
    [SerializeField]
    private Options options;

    #region Lifecycle
    private void Awake()
    {
        Fullscreen = options.isFullscreen;

        MasterVolume = options.masterVolumeLevel;
        SoundFXVolume = options.sfxVolumeLevel;
        MusicVolume = options.musicVolumeLevel;

        SensitivityX = options.sensitivityX;
        SensitivityY = options.sensitivityY;

        FieldOfView = options.fieldOfView;
    }
    #endregion

    #region Fullscreen
    [SerializeField]
    private Toggle fullscreenToggle;

    [SerializeField]
    private bool fullscreen;
    public bool Fullscreen
    {
        get => fullscreen;
        set
        {
            fullscreen = value;
            Screen.fullScreen = fullscreen;
            options.isFullscreen = fullscreen;
            fullscreenToggle.isOn = fullscreen;
            Cursor.lockState = fullscreen ? CursorLockMode.Confined : CursorLockMode.None;
        }
    }
    #endregion

    #region Volume
    [SerializeField]
    private AudioMixer audioMixer;

    private float CalculateVolumen(float volume) => Mathf.Log10(volume) * 20f;

    [SerializeField]
    private float masterVolume;
    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = value;
            options.masterVolumeLevel = masterVolume;
            audioMixer.SetFloat(AudioMixerStrings.MasterVolume, CalculateVolumen(masterVolume));
        }
    }

    [SerializeField]
    private float soundFXVolume;
    public float SoundFXVolume
    {
        get => soundFXVolume;
        set
        {
            soundFXVolume = value;
            options.sfxVolumeLevel = soundFXVolume;
            audioMixer.SetFloat(AudioMixerStrings.SoundFXVolume, CalculateVolumen(soundFXVolume));
        }
    }

    [SerializeField]
    private float musicVolume;
    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            options.musicVolumeLevel = musicVolume;
            audioMixer.SetFloat(AudioMixerStrings.MusicVolume, CalculateVolumen(musicVolume));
        }
    }
    #endregion

    #region Sensitivity
    [SerializeField]
    private float sensitivityX;
    public float SensitivityX
    {
        get => sensitivityX;
        set
        {
            sensitivityX = value;
            options.sensitivityX = sensitivityX;
        }
    }

    [SerializeField]
    private float sensitivityY;
    public float SensitivityY
    {
        get => sensitivityY;
        set
        {
            sensitivityY = value;
            options.sensitivityY = sensitivityY;
        }
    }
    #endregion

    #region Field of View
    [SerializeField]
    private float fieldOfView;
    public float FieldOfView
    {
        get => fieldOfView;
        set
        {
            fieldOfView = value;
            options.fieldOfView = fieldOfView;
        }
    }
    #endregion
}

public class AudioMixerStrings
{
    internal static string MasterVolume = "masterVolume";
    internal static string SoundFXVolume = "soundFXVolume";
    internal static string MusicVolume = "musicVolume";
}
