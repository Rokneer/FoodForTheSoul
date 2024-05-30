using DG.Tweening;
using UnityEngine;

public class PhotoZoom : MonoBehaviour
{
    #region Variables
    [Header("Cameras")]
    [SerializeField]
    private Camera photoCamera;

    private FirstPersonCamera cameraController;

    [Header("Positions")]
    [SerializeField]
    private Vector3 basePosition = new(0.23f, -0.141f, 0.738f);

    [SerializeField]
    private Vector3 zoomPosition = new(0.364f, -0.339f, 0.734f);

    [Header("Zoom")]
    [SerializeField]
    private bool isZooming = false;
    public bool IsZooming
    {
        get => isZooming;
        set
        {
            isZooming = value;
            if (!isZooming)
            {
                isExtraZooming = false;
            }
        }
    }
    public bool isExtraZooming = false;

    [Header("Main Camera Values")]
    [SerializeField]
    private float mainCameraZoomInValue;

    [SerializeField]
    private float mainCameraExtraZoomValue;
    private float baseMainCameraFOV;

    [Header("Photo Camera Values")]
    [SerializeField]
    private float photoCameraZoomInValue;

    [SerializeField]
    private float photoCameraExtraZoomValue;
    private float basePhotoCameraFOV;

    private float baseSenXValue;
    private float ZoomInSenXValue => baseSenXValue / 3;
    private float ExtraZoomSenXValue => baseSenXValue / 5;
    private float baseSenYValue;
    private float ZoomInSenYValue => baseSenYValue / 3;
    private float ExtraZoomSenYValue => baseSenYValue / 5;

    [Header("Audio")]
    [SerializeField]
    private AudioClip zoomInSFX;

    [SerializeField]
    private AudioClip zoomOutSFX;

    private float mainCameraTargetFOV;
    private float photoCameraTargetFOV;
    private readonly float tweenDuration = 0.2f;
    #endregion Variables

    #region Lifecycle

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<FirstPersonCamera>();
        baseSenXValue = cameraController.sensX;
        baseSenYValue = cameraController.sensY;

        baseMainCameraFOV = Camera.main.fieldOfView;
        mainCameraTargetFOV = baseMainCameraFOV;

        basePhotoCameraFOV = photoCamera.fieldOfView;
        photoCameraTargetFOV = basePhotoCameraFOV;
    }
    #endregion Lifecycle

    #region Functions
    public void ZoomCamera(float mainFOV, float photoFOV, float senX, float senY, AudioClip sfx)
    {
        SoundFXManager.Instance.PlaySoundFXClip(sfx, transform, 1);

        Camera.main.DOFieldOfView(mainFOV, tweenDuration);
        photoCamera.DOFieldOfView(photoFOV, tweenDuration);

        cameraController.sensX = senX;
        cameraController.sensY = senY;
    }

    public void ZoomIn()
    {
        transform.localPosition = basePosition;
        ZoomCamera(
            mainCameraZoomInValue,
            photoCameraZoomInValue,
            ZoomInSenXValue,
            ZoomInSenYValue,
            zoomInSFX
        );
    }

    public void ExtraZoomIn()
    {
        ZoomCamera(
            mainCameraExtraZoomValue,
            photoCameraExtraZoomValue,
            ExtraZoomSenXValue,
            ExtraZoomSenYValue,
            zoomInSFX
        );
    }

    public void ZoomOut()
    {
        transform.localPosition = zoomPosition;
        ZoomCamera(baseMainCameraFOV, basePhotoCameraFOV, baseSenXValue, baseSenYValue, zoomOutSFX);
    }
    #endregion Functions
}
