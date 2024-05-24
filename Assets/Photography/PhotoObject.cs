using UnityEngine;

public abstract class PhotoObject : MonoBehaviour
{
    #region Variables
    public PhotoObjectData data;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    #endregion Variables

    #region Lifecycle

    #endregion Lifecycle

    #region Functions
    public void SetMeshData()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = data.meshFilter.GetComponent<MeshFilter>().sharedMesh;

        meshRenderer.materials = data.meshFilter.GetComponent<MeshRenderer>().sharedMaterials;
    }

    public virtual void WasPhotographed()
    {
        ObjectPoolManager.ReturnToPool(gameObject);
    }

    #endregion Functions
}
