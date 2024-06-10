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

    public Tween tween;
    #endregion

    #region Functions
    internal void SetUpMovement(Transform startPoint, Transform endPoint)
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

    internal void StartMovement(
        Ease ease = Ease.InOutSine,
        int loopCount = 0,
        LoopType loopType = LoopType.Yoyo
    )
    {
        tween = transform
            .DOMove(endTransform.position, tweenTime)
            .SetLoops(loopCount, loopType)
            .SetEase(ease)
            .SetAutoKill(false)
            .OnComplete(FinishTween);
    }

    internal void ReverseMovement()
    {
        tween.Pause().PlayBackwards();
    }

    internal void PauseMovement()
    {
        tween.Pause();
    }

    internal void ResumeMovement()
    {
        tween.Play();
    }

    internal void FinishTween()
    {
        tween.Kill();
        ResetTransforms();
    }

    private void ResetTransforms()
    {
        startTransform = null;
        endTransform = null;
    }
    #endregion
}
