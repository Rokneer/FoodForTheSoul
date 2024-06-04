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

    public void StartMovement(
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

    public void ReverseMovement()
    {
        tween.Pause().PlayBackwards();
    }

    public void PauseMovement()
    {
        tween.Pause();
    }

    public void ResumeMovement()
    {
        tween.Play();
    }

    public void FinishTween()
    {
        tween.Kill();
        ResetTransforms();
    }

    private void ResetTransforms()
    {
        startTransform = null;
        endTransform = null;
    }
    #endregion Functions
}
