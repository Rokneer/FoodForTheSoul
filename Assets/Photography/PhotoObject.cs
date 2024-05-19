using DG.Tweening;
using UnityEngine;

public class PhotoObject : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private PhotoObjectData data;
    #endregion Variables

    #region Lifecycle

    #endregion Lifecycle

    #region Functions
    public void WasPhotographed()
    {
        Debug.Log(data.objectName);
        Destroy(gameObject);
    }
    #endregion Functions
}
