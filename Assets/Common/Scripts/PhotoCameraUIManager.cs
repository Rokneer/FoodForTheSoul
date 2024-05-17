using System.Collections;
using System.Collections.Generic;
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

    private List<Animator> photoAnimators = new();

    [SerializeField]
    private int activePhotoIndex = 0;

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
            photoAnimators.Add(photoFrame.GetComponent<Animator>());
        }
    }
    #endregion Lifecycle

    #region Functions
    public IEnumerator AddPhoto(Sprite photoSprite)
    {
        if (activePhotoIndex == 3)
        {
            activePhotoIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(HidePhoto(i));
            }
        }

        photoDisplayImages[activePhotoIndex].sprite = photoSprite;
        photoFrames[activePhotoIndex].SetActive(true);

        photoAnimators[activePhotoIndex].Play(AnimationStrings.PhotoSlide);
        yield return new WaitForSeconds(1);
        photoAnimators[activePhotoIndex].Play(AnimationStrings.PhotoFade);

        activePhotoIndex++;
    }

    private IEnumerator HidePhoto(int photoIndex)
    {
        photoAnimators[photoIndex].Play(AnimationStrings.PhotoSlide);

        photoFrames[photoIndex].SetActive(false);
        yield return new WaitForSeconds(1);
    }
    #endregion Functions
}
