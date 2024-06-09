using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Options")]
public class Options : ScriptableObject
{
    [Header("Fullscreen")]
    public bool isFullscreen = true;

    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolumeLevel = 0.7f;

    [Range(0, 1)]
    public float sfxVolumeLevel = 0.7f;

    [Range(0, 1)]
    public float musicVolumeLevel = 0.7f;

    [Header("Sensitivity")]
    [Range(0, 1)]
    public float sensitivityX = 0.7f;

    [Range(0, 1)]
    public float sensitivityY = 0.7f;

    [Header("FOV")]
    [Range(40, 90)]
    public int fieldOfView = 80;
}
