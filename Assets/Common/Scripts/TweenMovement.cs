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
    [SerializeField]
    private float tweenTime;

    [SerializeField]
    private int loopCount;
    #endregion Variables

    #region Lifecycle
    private void Awake()
    {
        if (endTransform != null)
        {
            if (startTransform == null)
            {
                startTransform = transform;
            }
            transform.position = startTransform.position;
            MoveToPosition();
        }
    }
    #endregion Lifecycle

    #region Functions
    private void MoveToPosition()
    {
        transform
            .DOMove(endTransform.position, tweenTime)
            .SetLoops(loopCount, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        ;
    }
    #endregion Functions
}
