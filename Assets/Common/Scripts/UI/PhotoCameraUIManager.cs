using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PhotoCameraUIManager : PhotoUIManager<PhotoCameraUIManager>
{
    #region Variables
    [SerializeField]
    private List<TMP_Text> ingredientText;

    [SerializeField]
    private GameObject reloadUI;

    public override int ActivePhotoCount
    {
        get => base.ActivePhotoCount;
        set
        {
            base.ActivePhotoCount = value;
            if (base.ActivePhotoCount > -1)
            {
                reloadUI.SetActive(true);
            }
            else
            {
                reloadUI.SetActive(false);
            }
        }
    }
    #endregion

    #region Functions
    internal void AddPhoto(Sprite photoSprite, string photoLabel)
    {
        photoLabel ??= "Nothing :c";

        ActivePhotoCount++;

        // Set photo sprite
        displayImages[ActivePhotoCount].sprite = photoSprite;

        // Set ingredient label
        ingredientText[ActivePhotoCount].text = photoLabel;

        // Slide photo into frame
        photoFramesRectTransforms[ActivePhotoCount]
            .DOAnchorPosX(slideInPoint, slideInTime)
            .SetEase(Ease.InOutSine);

        // Fade photo in
        photoCanvasGroups[ActivePhotoCount].DOFade(1, fadeInTime);
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
        ActivePhotoCount = -1;
        for (int i = 0; i < 3; i++)
        {
            HidePhoto(i);
        }
        // Remove all current ingredients
        RecipeManager.Instance.ResetCurrentIngredients();
    }
    #endregion
}
