using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : PhotoUIManager<RecipeUIManager>
{
    #region Variables
    [Header("Timer")]
    [SerializeField]
    private Slider[] recipeTimers = new Slider[3];
    private readonly Tween[] timerTweens = new Tween[3];
    private readonly float[] targetTimerValues = new float[3];
    private readonly float[] timerDurations = new float[3];
    private readonly Dictionary<int, bool> activeTimers = new(3);

    [Header("Completed Recipe Frame")]
    public RecipeData equipedRecipe;

    [SerializeField]
    private float slideInRecipePoint;

    [SerializeField]
    private float slideOutRecipePoint;

    [SerializeField]
    private Image completedRecipeDisplayImage;

    [SerializeField]
    private CanvasGroup completedRecipeCanvasGroup;

    [SerializeField]
    private RectTransform completedRecipeRectTransform;

    [SerializeField]
    private TMP_Text completedRecipeText;

    [Header("Ingredient Frames")]
    [SerializeField]
    private GameObject[] ingredientFrames;

    [SerializeField]
    private List<ListWrapper<Image>> ingredientDisplayImages;

    [SerializeField]
    private List<ListWrapper<CanvasGroup>> ingredientCanvasGroups;

    #endregion

    #region Lifecycle
    protected override void Awake()
    {
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
    #endregion

    #region Functions
    internal void ShowRecipePhoto(RecipeData recipe)
    {
        equipedRecipe = recipe;

        // Set recipe sprite
        completedRecipeDisplayImage.sprite = recipe.sprite;

        // Set recipe label
        completedRecipeText.text = recipe.label;

        completedRecipeRectTransform
            .DOAnchorPosY(slideInRecipePoint, slideInTime)
            .SetEase(Ease.InOutSine);

        // Fade recipe photo in
        completedRecipeCanvasGroup.DOFade(1, fadeInTime);
    }

    internal void HideRecipePhoto()
    {
        if (equipedRecipe != null)
        {
            equipedRecipe = null;
            completedRecipeRectTransform
                .DOAnchorPosY(slideOutRecipePoint, slideOutTime)
                .SetEase(Ease.InOutSine);

            // Fade recipe photo in
            completedRecipeCanvasGroup.DOFade(1, fadeOutTime);
        }
    }

    internal int AddPhoto(Sprite photoSprite, List<Sprite> ingredientSprites, int id)
    {
        displayImages[id].sprite = photoSprite;

        List<Image> currentImages = ingredientDisplayImages[id].innerList;
        for (int i = 0; i < ingredientSprites.Count; i++)
        {
            Sprite sprite = ingredientSprites[i];
            currentImages[i].sprite = sprite;
        }

        // Slide photo into frame
        photoFramesRectTransforms[id]
            .DOAnchorPosY(slideInPoint, slideInTime)
            .SetEase(Ease.InOutSine);

        // Fade photo and ingredients in
        photoCanvasGroups[id].DOFade(1, fadeInTime);
        List<CanvasGroup> currentCanvasGroups = ingredientCanvasGroups[id].innerList;
        foreach (CanvasGroup canvasGroup in currentCanvasGroups)
        {
            canvasGroup.DOFade(1, fadeInTime);
        }

        return activePhotoCount;
    }

    internal override void HidePhoto(int photoId)
    {
        // Slide out of frame
        photoFramesRectTransforms[photoId]
            .DOAnchorPosY(slideOutPoint, slideOutTime)
            .SetEase(Ease.InOutSine);
        base.HidePhoto(photoId);
    }

    internal void SetupTimer(float timerValue, float tweenDuration, int id)
    {
        // Set tween duration
        timerDurations[id] = tweenDuration;

        // Set slider max and current value
        recipeTimers[id].maxValue = timerValue;
        recipeTimers[id].value = timerValue;

        // Set target value
        targetTimerValues[id] = 0;

        // Activate timer
        activeTimers[id] = true;
    }

    internal void StartTimer(int id)
    {
        timerTweens[id] = recipeTimers[id]
            .DOValue(targetTimerValues[id], timerDurations[id])
            .SetEase(Ease.InOutSine)
            .SetAutoKill(false);
    }

    internal void PauseTimer(int id)
    {
        timerTweens[id].Pause();
    }

    internal void ResumeTimer(int id)
    {
        timerTweens[id].Play();
    }

    internal void DisableTimer(int id)
    {
        PauseTimer(id);
        timerTweens[id].Kill(false);
        activeTimers[id] = false;
    }
    #endregion
}
