using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : Singleton<SettingsMenu>
{
    [SerializeField]
    private Settings settings;

    #region Lifecycle
    private void Awake()
    {
        Fullscreen = settings.isFullscreen;

        MasterVolume = settings.masterVolumeLevel;
        SoundFXVolume = settings.sfxVolumeLevel;
        MusicVolume = settings.musicVolumeLevel;

        SensitivityX = settings.sensitivityX;
        SensitivityY = settings.sensitivityY;

        FieldOfView = settings.fieldOfView;
    }
    #endregion

    #region Fullscreen
    [Header("Fullscreen")]
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
            settings.isFullscreen = fullscreen;
            fullscreenToggle.isOn = fullscreen;
            Cursor.lockState = fullscreen ? CursorLockMode.Confined : CursorLockMode.None;
        }
    }
    #endregion

    #region Volume
    [Header("Audio")]
    [SerializeField]
    private AudioMixer audioMixer;

    private float CalculateVolumen(float volume) => Mathf.Log10(volume) * 20f;

    [Space]
    [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI masterVolumeText;

    [SerializeField]
    private float masterVolume;
    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = value;
            settings.masterVolumeLevel = masterVolume;
            masterVolumeSlider.value = masterVolume;
            masterVolumeText.text = masterVolume.ToString();
            audioMixer.SetFloat(AudioMixerStrings.MasterVolume, CalculateVolumen(masterVolume));
        }
    }

    [Space]
    [SerializeField]
    private Slider soundFXVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI soundFXVolumeText;

    [SerializeField]
    private float soundFXVolume;

    public float SoundFXVolume
    {
        get => soundFXVolume;
        set
        {
            soundFXVolume = value;
            settings.sfxVolumeLevel = soundFXVolume;
            soundFXVolumeSlider.value = soundFXVolume;
            soundFXVolumeText.text = soundFXVolume.ToString();
            audioMixer.SetFloat(AudioMixerStrings.SoundFXVolume, CalculateVolumen(soundFXVolume));
        }
    }

    [Space]
    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI musicVolumeText;

    [SerializeField]
    private float musicVolume;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            settings.musicVolumeLevel = musicVolume;
            musicVolumeSlider.value = musicVolume;
            musicVolumeText.text = musicVolume.ToString();
            audioMixer.SetFloat(AudioMixerStrings.MusicVolume, CalculateVolumen(musicVolume));
        }
    }
    #endregion

    #region Sensitivity
    [Header("Sensitivity")]
    [SerializeField]
    private FirstPersonCamera fpsCamera;

    [Space]
    [SerializeField]
    private Slider sensitivityXSlider;

    [SerializeField]
    private TextMeshProUGUI sensitivityXText;

    [SerializeField]
    private float sensitivityX;
    public float SensitivityX
    {
        get => sensitivityX;
        set
        {
            sensitivityX = value;
            settings.sensitivityX = sensitivityX;
            if (fpsCamera != null)
            {
                fpsCamera.currentSenX = sensitivityX;
            }
            sensitivityXSlider.value = sensitivityX;
            sensitivityXText.text = sensitivityX.ToString();
        }
    }

    [Space]
    [SerializeField]
    private Slider sensitivityYSlider;

    [SerializeField]
    private TextMeshProUGUI sensitivityYText;

    [SerializeField]
    private float sensitivityY;
    public float SensitivityY
    {
        get => sensitivityY;
        set
        {
            sensitivityY = value;
            settings.sensitivityY = sensitivityY;
            if (fpsCamera != null)
            {
                fpsCamera.currentSenY = sensitivityY;
            }
            sensitivityYSlider.value = sensitivityY;
            sensitivityYText.text = sensitivityY.ToString();
        }
    }
    #endregion

    #region Field of View
    [Header("Field of View")]
    [SerializeField]
    private Slider fieldOfViewSlider;

    [SerializeField]
    private TextMeshProUGUI fieldOfViewText;

    [SerializeField]
    private float fieldOfView;
    public float FieldOfView
    {
        get => fieldOfView;
        set
        {
            fieldOfView = value;
            settings.fieldOfView = fieldOfView;
            fieldOfViewSlider.value = fieldOfView;
            fieldOfViewText.text = fieldOfView.ToString();
            Camera.main.fieldOfView = fieldOfView;
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
