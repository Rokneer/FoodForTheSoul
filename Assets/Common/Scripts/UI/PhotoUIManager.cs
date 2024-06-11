using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class PhotoUIManager<T> : Singleton<T>
    where T : MonoBehaviour
{
    #region Variables
    public int activePhotoCount = -1;

    [Header("Slide Animation")]
    [SerializeField]
    protected float slideInTime = 0.25f;

    [SerializeField]
    protected float slideOutTime = 0.25f;

    [Space]
    [SerializeField]
    protected float slideInPoint;

    [SerializeField]
    protected float slideOutPoint;

    [Header("Fade Animation")]
    [SerializeField]
    protected float fadeInTime = 1f;

    [SerializeField]
    protected float fadeOutTime = 0.1f;

    [Header("Photo Frames")]
    [SerializeField]
    protected GameObject[] photoFrames;

    [SerializeField]
    protected List<Image> displayImages;

    protected readonly List<RectTransform> photoFramesRectTransforms = new();
    protected readonly List<CanvasGroup> photoCanvasGroups = new();
    #endregion

    #region Lifecycle
    protected virtual void Awake()
    {
        // Gets photo frame components
        foreach (GameObject photoFrame in photoFrames)
        {
            photoFramesRectTransforms.Add(photoFrame.GetComponent<RectTransform>());
            photoCanvasGroups.Add(photoFrame.GetComponentInChildren<CanvasGroup>());
        }
    }
    #endregion

    #region Functions
    internal virtual void HidePhoto(int photoId)
    {
        // Fade photo out
        photoCanvasGroups[photoId].DOFade(0, fadeOutTime);
    }
    #endregion
}
