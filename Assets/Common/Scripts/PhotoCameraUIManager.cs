using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCameraUIManager : MonoBehaviour
{
    #region Variables
    private static PhotoCameraUIManager _instance;
    public static PhotoCameraUIManager Instance => _instance;

    [Header("Photo Frames")]
    [SerializeField]
    private GameObject[] photoFrames;

    [SerializeField]
    private List<Image> photoDisplayImages;

    [SerializeField]
    private List<Sprite> photoDisplaySprites;

    private readonly List<RectTransform> photoFramesRectTransforms = new();
    private readonly List<CanvasGroup> photoCanvasGroups = new();

    public int activePhotoIndex = 0;
    private readonly float fadeTime = 1f;
    #endregion Variables

    #region Lifecycle
    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // Gets photo frame animators
        foreach (GameObject photoFrame in photoFrames)
        {
            photoFramesRectTransforms.Add(photoFrame.GetComponent<RectTransform>());
            photoCanvasGroups.Add(photoFrame.GetComponentInChildren<CanvasGroup>());
        }
    }
    #endregion Lifecycle

    #region Functions
    public void AddPhoto(Sprite photoSprite)
    {
        photoDisplaySprites.Add(photoSprite);
        photoDisplayImages[activePhotoIndex].sprite = photoSprite;

        // Slide into frame
        photoFramesRectTransforms[activePhotoIndex]
            .DOAnchorPosX(-157, 0.25f)
            .SetEase(Ease.InOutSine);
        // Fade photo in
        photoCanvasGroups[activePhotoIndex].DOFade(1, fadeTime);

        activePhotoIndex++;
    }

    private void HidePhoto(int photoIndex)
    {
        // Fade photo out
        photoCanvasGroups[photoIndex].DOFade(0, 0.1f);
        // Slide out of frame
        photoFramesRectTransforms[photoIndex].DOAnchorPosX(157, 0.25f).SetEase(Ease.InOutSine);
    }

    public void ResetPhotos()
    {
        activePhotoIndex = 0;
        photoDisplaySprites.Clear();
        for (int i = 0; i < 3; i++)
        {
            HidePhoto(i);
        }
    }
    #endregion Functions
}
