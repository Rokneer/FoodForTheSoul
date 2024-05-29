using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PhotoCameraUIManager : PhotoUIManager
{
    #region Variables
    private static PhotoCameraUIManager _instance;
    public static PhotoCameraUIManager Instance => _instance;

    [SerializeField]
    private List<TMP_Text> ingredientText;
    #endregion Variables

    #region Lifecycle
    protected override void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        base.Awake();
    }
    #endregion Lifecycle

    #region Functions
    public void AddPhoto(Sprite photoSprite, string photoLabel)
    {
        photoLabel ??= "Nothing :c";

        activePhotoIndex++;

        // Set photo sprite
        displayImages[activePhotoIndex].sprite = photoSprite;

        // Set ingredient label
        ingredientText[activePhotoIndex].text = photoLabel;

        // Slide photo into frame
        photoFramesRectTransforms[activePhotoIndex]
            .DOAnchorPosX(slideInPoint, slideInTime)
            .SetEase(Ease.InOutSine);

        // Fade photo in
        photoCanvasGroups[activePhotoIndex].DOFade(1, fadeInTime);
    }

    public override void HidePhoto(int photoIndex)
    {
        base.HidePhoto(photoIndex);

        // Slide out of frame
        photoFramesRectTransforms[photoIndex]
            .DOAnchorPosX(slideOutPoint, slideOutTime)
            .SetEase(Ease.InOutSine);
    }

    public void ResetPhotos()
    {
        // Reset photo to inactive state
        activePhotoIndex = -1;
        for (int i = 0; i < 3; i++)
        {
            HidePhoto(i);
        }
        // Remove all current ingredients
        RecipeManager.Instance.ResetCurrentIngredients();
    }
    #endregion Functions
}
