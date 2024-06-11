using DG.Tweening;
using UnityEngine;

public class PhotoZoom : MonoBehaviour
{
    #region Variables
    private Camera photoCamera;
    private FirstPersonCamera cameraController;

    [Header("Positions")]
    [SerializeField]
    private Vector3 basePosition = new();

    [SerializeField]
    private Vector3 zoomPosition = new();

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

    private float BaseMainCamFOV => SettingsMenu.Instance.FieldOfView;
    private float MainCamZoomFOV => BaseMainCamFOV / 3;
    private float MainCamExtraZoomFOV => BaseMainCamFOV / 5;

    private float BasePhotoCameraFOV => SettingsMenu.Instance.FieldOfView / 5;
    private float PhotoCamZoomInFOV => BasePhotoCameraFOV / 3;
    private float PhotoCamExtraZoomFOV => BasePhotoCameraFOV / 5;

    private float BaseSenX => SettingsMenu.Instance.SensitivityX;
    private float ZoomInSenX => BaseSenX / 3;
    private float ExtraZoomSenX => BaseSenX / 5;

    private float BaseSenY => SettingsMenu.Instance.SensitivityY;
    private float ZoomInSenY => BaseSenY / 3;
    private float ExtraZoomSenY => BaseSenY / 5;

    [Header("Audio")]
    [SerializeField]
    private AudioClip zoomInSFX;

    [SerializeField]
    private AudioClip zoomOutSFX;

    private readonly float tweenDuration = 0.2f;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<FirstPersonCamera>();
        photoCamera = GetComponentInChildren<Camera>();
    }
    #endregion

    #region Zoom
    internal void ZoomCamera(float mainFOV, float photoFOV, float senX, float senY, AudioClip sfx)
    {
        SoundFXManager.Instance.PlaySFXClip(sfx, transform, 0.2f);

        Camera.main.DOFieldOfView(mainFOV, tweenDuration);
        photoCamera.DOFieldOfView(photoFOV, tweenDuration);

        cameraController.currentSenX = senX;
        cameraController.currentSenY = senY;
    }

    internal void ZoomIn()
    {
        UIManager.Instance.HideUI(UITypes.Recipes);
        UIManager.Instance.HideUI(UITypes.Score);
        UIManager.Instance.ShowUI(UITypes.Zoom);

        transform.localPosition = basePosition;
        ZoomCamera(MainCamZoomFOV, PhotoCamZoomInFOV, ZoomInSenX, ZoomInSenY, zoomInSFX);
    }

    internal void ExtraZoomIn()
    {
        UIManager.Instance.HideUI(UITypes.Zoom);

        ZoomCamera(
            MainCamExtraZoomFOV,
            PhotoCamExtraZoomFOV,
            ExtraZoomSenX,
            ExtraZoomSenY,
            zoomInSFX
        );
    }

    internal void ZoomOut()
    {
        UIManager.Instance.HideUI(UITypes.Zoom);
        UIManager.Instance.ShowUI(UITypes.Recipes);
        UIManager.Instance.ShowUI(UITypes.Score);

        transform.localPosition = zoomPosition;
        ZoomCamera(BaseMainCamFOV, BasePhotoCameraFOV, BaseSenX, BaseSenY, zoomOutSFX);
    }
    #endregion
}
