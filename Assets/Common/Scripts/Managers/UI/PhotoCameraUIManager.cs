using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PhotoCameraUIManager : PhotoUIManager
{
    #region Variables
    public static PhotoCameraUIManager Instance { get; private set; }

    [SerializeField]
    private List<TMP_Text> ingredientText;
    #endregion Variables

    #region Lifecycle
    protected override void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        base.Awake();
    }
    #endregion Lifecycle

    #region Functions
    internal void AddPhoto(Sprite photoSprite, string photoLabel)
    {
        photoLabel ??= "Nothing :c";

        activePhotoCount++;

        // Set photo sprite
        displayImages[activePhotoCount].sprite = photoSprite;

        // Set ingredient label
        ingredientText[activePhotoCount].text = photoLabel;

        // Slide photo into frame
        photoFramesRectTransforms[activePhotoCount]
            .DOAnchorPosX(slideInPoint, slideInTime)
            .SetEase(Ease.InOutSine);

        // Fade photo in
        photoCanvasGroups[activePhotoCount].DOFade(1, fadeInTime);
    }

    internal override void HidePhoto(int photoId)
    {
        base.HidePhoto(photoId);

        // Slide out of frame
        photoFramesRectTransforms[photoId]
            .DOAnchorPosX(slideOutPoint, slideOutTime)
            .SetEase(Ease.InOutSine);
    }

    internal void ResetPhotos()
    {
        // Reset photo to inactive state
        activePhotoCount = -1;
        for (int i = 0; i < 3; i++)
        {
            HidePhoto(i);
        }
        // Remove all current ingredients
        RecipeManager.Instance.ResetCurrentIngredients();
    }
    #endregion Functions
}
