using System.Collections;
using UnityEngine;

public class PhotoCapture : MonoBehaviour
{
    #region Variables
    [Header("Photo Taker")]
    [SerializeField]
    private GameObject cameraCanvas;

    [SerializeField]
    private float photoDelay;

    public bool canTakePhoto = true;
    private Texture2D screenCapture;
    private int PhotoWidth => Screen.width / 4;
    private int PhotoHeight => Screen.height / 4;

    [Header("Audio")]
    [SerializeField]
    private AudioClip photoSFX;

    [Header("Visibility")]
    private bool _isCameraCanvasVisible;
    public bool IsCameraCanvasVisible
    {
        get => _isCameraCanvasVisible;
        set
        {
            _isCameraCanvasVisible = value;
            cameraCanvas.SetActive(_isCameraCanvasVisible);
        }
    }
    #endregion Variables

    #region Lifecycle
    private void Start()
    {
        screenCapture = new Texture2D(PhotoWidth, PhotoHeight, TextureFormat.RGB24, false);
    }
    #endregion Lifecycle

    #region Functions
    public IEnumerator CapturePhoto()
    {
        canTakePhoto = false;
        yield return new WaitForEndOfFrame();
        Rect regionToRead = new(0, 0, PhotoWidth, PhotoHeight);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        Sprite photoSprite = Sprite.Create(
            screenCapture,
            new Rect(0.0f, 0.0f, PhotoWidth, PhotoHeight),
            new Vector2(0.5f, 0.5f),
            100.0f
        );
        StartCoroutine(PhotoCameraUIManager.Instance.AddPhoto(photoSprite));
        yield return new WaitForSeconds(photoDelay);
        canTakePhoto = true;
    }

    #endregion Functions
}
