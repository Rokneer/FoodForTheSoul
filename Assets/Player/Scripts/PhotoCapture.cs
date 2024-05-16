using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField]
    private Image photoDisplayArea;

    [SerializeField]
    private GameObject photoFrame;

    [SerializeField]
    private GameObject cameraCrosshair;

    [SerializeField]
    private GameObject cameraCanvas;
    private Texture2D screenCapture;

    [Header("Flash Effect")]
    [SerializeField]
    private GameObject cameraFlash;

    [SerializeField]
    private float flashTime;

    [Header("Photo Fader Effect")]
    [SerializeField]
    private Animator fadingAnimation;

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
    public bool isViewingPhoto;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    public IEnumerator CapturePhoto()
    {
        cameraCrosshair.SetActive(false);
        isViewingPhoto = true;

        yield return new WaitForEndOfFrame();
        Rect regionToRead = new(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
    }

    private void ShowPhoto()
    {
        StartCoroutine(CameraFlashEffect());
        Sprite photoSprite = Sprite.Create(
            screenCapture,
            new Rect(0.0f, 0.0f, Screen.width, Screen.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);

        fadingAnimation.Play(AnimationStrings.PhotoFade);
    }

    private IEnumerator CameraFlashEffect()
    {
        SoundFXManager.Instance.PlaySoundFXClip(photoSFX, transform, 1f);
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    public void RemovePhoto()
    {
        isViewingPhoto = false;
        photoFrame.SetActive(false);
        cameraCrosshair.SetActive(true);
    }
}
