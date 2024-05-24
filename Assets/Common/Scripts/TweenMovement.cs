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

    public void StartMovement()
    {
        transform
            .DOMove(endTransform.position, tweenTime)
            .SetLoops(loopCount, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(OnTweenEnd);
    }

    public void StopMovement()
    {
        //* Stop tween movement
    }

    private void OnTweenEnd()
    {
        if (gameObject.TryGetComponent<Ingredient>(out _))
        {
            ResetTransforms();
            FoodSpawnManager.Instance.RemoveObject(gameObject);
        }
        else if (gameObject.TryGetComponent<Creature>(out _))
        {
            ResetTransforms();
            CreatureSpawnManager.Instance.RemoveObject(gameObject);
        }
        else if (gameObject.TryGetComponent<Customer>(out _))
        {
            ResetTransforms();
            CustomerSpawnManager.Instance.RemoveObject(gameObject);
        }
    }

    private void ResetTransforms()
    {
        startTransform = null;
        endTransform = null;
    }
    #endregion Functions
}
