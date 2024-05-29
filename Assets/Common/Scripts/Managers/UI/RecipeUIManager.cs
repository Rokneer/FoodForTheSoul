using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : PhotoUIManager
{
    #region Variables
    private static RecipeUIManager _instance;
    public static RecipeUIManager Instance => _instance;

    [Header("Ingredient Frames")]
    [SerializeField]
    private GameObject[] ingredientFrames;

    [SerializeField]
    private List<ListWrapper<Image>> ingredientDisplayImages;

    [SerializeField]
    private List<ListWrapper<CanvasGroup>> ingredientCanvasGroups;

    #endregion Variables


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

        // Gets photo frame components
        foreach (GameObject photoFrame in photoFrames)
        {
            photoFramesRectTransforms.Add(photoFrame.GetComponent<RectTransform>());
            photoCanvasGroups.Add(photoFrame.GetComponentInChildren<CanvasGroup>());
        }
        for (int i = 0; i < ingredientFrames.Length; i++)
        {
            GameObject ingredientFrame = ingredientFrames[i];

            CanvasGroup[] canvasGroups = ingredientFrame.GetComponentsInChildren<CanvasGroup>();

            foreach (CanvasGroup canvasGroup in canvasGroups)
            {
                ingredientCanvasGroups[i].innerList.Add(canvasGroup);
            }
        }
    }

    #region Functions
    public int AddPhoto(Sprite photoSprite, Sprite[] ingredientSprites, int index)
    {
        displayImages[index].sprite = photoSprite;

        List<Image> currentImages = ingredientDisplayImages[index].innerList;
        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            Sprite sprite = ingredientSprites[i];
            currentImages[i].sprite = sprite;
        }

        // Slide photo into frame
        photoFramesRectTransforms[index]
            .DOAnchorPosY(slideInPoint, slideInTime)
            .SetEase(Ease.InOutSine);

        // Fade photo and ingredients in
        photoCanvasGroups[index].DOFade(1, fadeInTime);
        List<CanvasGroup> currentCanvasGroups = ingredientCanvasGroups[index].innerList;
        foreach (CanvasGroup canvasGroup in currentCanvasGroups)
        {
            canvasGroup.DOFade(1, fadeInTime);
        }

        return activePhotoIndex;
    }

    public override void HidePhoto(int photoIndex)
    {
        // Slide out of frame
        photoFramesRectTransforms[photoIndex]
            .DOAnchorPosY(slideOutPoint, slideOutTime)
            .SetEase(Ease.InOutSine);
        base.HidePhoto(photoIndex);
    }
    #endregion Functions
}

[System.Serializable]
public class ListWrapper<T> : List<T>
    where T : Object
{
    public List<T> innerList;
}
