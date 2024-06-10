using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Options")]
public class Options : ScriptableObject
{
    [Header("Fullscreen")]
    public bool isFullscreen = true;

    [Header("Volume")]
    const float minVolume = 0.0001f;
    const float maxVolume = 1f;

    [Range(minVolume, maxVolume)]
    public float masterVolumeLevel = 0.7f;

    [Range(minVolume, maxVolume)]
    public float sfxVolumeLevel = 0.7f;

    [Range(minVolume, maxVolume)]
    public float musicVolumeLevel = 0.7f;

    [Header("Sensitivity")]
    const float minSensitivity = 1f;
    const float maxSensitivity = 8f;

    [Range(minSensitivity, maxSensitivity)]
    public float sensitivityY = 4;

    [Range(minSensitivity, maxSensitivity)]
    public float sensitivityX = 5;

    [Header("FOV")]
    const float minFOV = 40f;
    const float maxFOV = 90f;

    [Range(minFOV, maxFOV)]
    public float fieldOfView = 80;
}
