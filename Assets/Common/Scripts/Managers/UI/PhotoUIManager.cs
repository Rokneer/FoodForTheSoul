using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class PhotoUIManager : MonoBehaviour
{
    #region Variables

    [Header("Photo Frames")]
    [SerializeField]
    protected GameObject[] photoFrames;

    [SerializeField]
    protected List<Image> displayImages;

    protected readonly List<RectTransform> photoFramesRectTransforms = new();
    protected readonly List<CanvasGroup> photoCanvasGroups = new();

    [HideInInspector]
    public int activePhotoIndex = 0;

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

    #endregion Variables

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
    #endregion Lifecycle

    #region Functions
    public virtual void AddPhoto(Sprite photoSprite) { }

    public virtual void AddPhoto(Sprite photoSprite, Sprite[] ingredientSprites) { }

    protected virtual void HidePhoto(int photoIndex)
    {
        // Fade photo out
        photoCanvasGroups[photoIndex].DOFade(0, fadeOutTime);
    }
    #endregion Functions
}
