using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : PhotoUIManager
{
    #region Variables
    private static RecipeUIManager _instance;
    public static RecipeUIManager Instance => _instance;

    [Header("Timer")]
    [SerializeField]
    private List<Slider> recipeTimers = new(3);

    [SerializeField]
    private float[] targetTimerValues = new float[3];

    [SerializeField]
    private float[] targetTimerValue = new float[3];

    [SerializeField]
    private float[] timerLerpSpeeds = new float[3];
    private Dictionary<int, bool> activeTimers = new();

    [Header("Ingredient Frames")]
    [SerializeField]
    private GameObject[] ingredientFrames;

    [SerializeField]
    private List<ListWrapper<Image>> ingredientDisplayImages;

    [SerializeField]
    private List<ListWrapper<CanvasGroup>> ingredientCanvasGroups;

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

        for (int i = 0; i < recipeTimers.Count; i++)
        {
            activeTimers[i] = false;
            targetTimerValues[i] = 0;
            targetTimerValue[i] = 0;
            timerLerpSpeeds[i] = 100;
        }
    }
    #endregion Lifecycle

    #region Functions
    public int AddPhoto(Sprite photoSprite, Sprite[] ingredientSprites, int id)
    {
        displayImages[id].sprite = photoSprite;

        List<Image> currentImages = ingredientDisplayImages[id].innerList;
        for (int i = 0; i < ingredientSprites.Length; i++)
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

    public override void HidePhoto(int photoId)
    {
        // Slide out of frame
        photoFramesRectTransforms[photoId]
            .DOAnchorPosY(slideOutPoint, slideOutTime)
            .SetEase(Ease.InOutSine);
        base.HidePhoto(photoId);
    }

    public void SetupTimer(float timerValue, float lerpSpeed, int id)
    {
        // Set lerp speed
        timerLerpSpeeds[id] = lerpSpeed;

        // Set slider max and current value
        recipeTimers[id].maxValue = timerValue;
        recipeTimers[id].value = timerValue;

        // Set target value
        targetTimerValues[id] = 0;

        // Activate timer
        activeTimers[id] = true;
    }

    public void StartTimer(int id)
    {
        recipeTimers[id].DOValue(targetTimerValues[id], timerLerpSpeeds[id]);
    }

    public void PauseTimer(int id)
    {
        targetTimerValues[id] = recipeTimers[id].value;
    }

    public void ResumeTimer(int id)
    {
        targetTimerValues[id] = 0;
    }

    public void DisableTimer(int id)
    {
        PauseTimer(id);
        activeTimers[id] = false;
    }
    #endregion Functions
}

[System.Serializable]
public class ListWrapper<T> : List<T>
    where T : Object
{
    public List<T> innerList;
}
