using UnityEngine;

public abstract class PhotoObject : MonoBehaviour
{
    #region Variables
    [SerializeField]
    internal PhotoObjectData data;

    [SerializeField]
    internal AudioClip soundEffect;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    #endregion

    #region Functions
    public void SetMeshData()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = data.model.GetComponent<MeshFilter>().sharedMesh;

        meshRenderer.materials = data.model.GetComponent<MeshRenderer>().sharedMaterials;
    }

    public virtual void WasPhotographed()
    {
        if (TryGetComponent<TweenMovement>(out TweenMovement tweenMovement))
        {
            tweenMovement.FinishTween();
            SoundFXManager.Instance.PlaySFXClip(soundEffect, transform, 0.3f);
        }
    }
    #endregion
}
