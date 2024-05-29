using DG.Tweening;
using UnityEngine;

public class TweenMovement : MonoBehaviour
{
    #region Variables
    [Header("Positions")]
    public Transform startTransform;
    public Transform endTransform;

    [SerializeField]
    private Vector3 currentPosition;

    [Header("Setup")]
    public float tweenTime;

    [SerializeField]
    private int loopCount;

    public Tween tween;
    #endregion Variables

    #region Functions
    public void SetUpMovement(Transform startPoint, Transform endPoint)
    {
        if (endPoint != null)
        {
            if (startPoint == null)
            {
                startPoint = transform;
            }
            startTransform = startPoint;
            endTransform = endPoint;
            transform.position = startPoint.position;
        }
    }

    public void StartMovement(Ease ease = Ease.InOutSine, LoopType loopType = LoopType.Yoyo)
    {
        tween = transform
            .DOMove(endTransform.position, tweenTime)
            .SetLoops(loopCount, loopType)
            .SetEase(ease)
            .OnComplete(ResetTransforms);
    }

    public Tween StartMovement(
        TweenCallback EndingAction,
        Ease ease = Ease.InOutSine,
        LoopType loopType = LoopType.Yoyo
    )
    {
        tween = transform
            .DOMove(endTransform.position, tweenTime)
            .SetLoops(loopCount, loopType)
            .SetEase(ease)
            .OnComplete(() =>
            {
                EndingAction();
            });
        return tween;
    }

    public Tween ReverseMovement()
    {
        tween.Pause().PlayBackwards();
        return tween;
    }

    public Tween PauseMovement()
    {
        tween.Pause();
        return tween;
    }

    public Tween ResumeMovement()
    {
        tween.Play();
        return tween;
    }

    private void ResetTransforms()
    {
        startTransform = null;
        endTransform = null;
    }
    #endregion Functions
}
